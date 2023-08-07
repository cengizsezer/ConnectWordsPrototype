using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using UnityEngine.UI.Extensions;
using System.Threading.Tasks;

public class UIPointer : Singleton<UIPointer>
{
    [SerializeField] InputManager InputManager;
   
    [SerializeField] Canvas _pointerCanvas;
    [SerializeField] ParticleSystem psClick;
    [SerializeField] GraphicRaycaster raycaster;
    BoardCell lastBoardSelected = null;
    public Lines activeLine = null;
    public Vector3 ScreenPointerPosition => InputManager.ScreenPointerPosition;
    public Vector3 CanvasPointerPosition => InputManager.CanvasPointerPosition;
    
    void Start()
    {
       
       
        _pointerCanvas.overrideSorting = true;
        _pointerCanvas.sortingOrder = 999;

        InputManager.e_OnPointerDown.AddListener(OnPointerDown);
        InputManager.e_OnDrag.AddListener(OnDrag);
        InputManager.e_OnPointerUp.AddListener(OnPointerUp);
       
    }
    void Update()
    {
        FollowMouse();
        InputManager.OnUpdate();
    }

    public void PlayParticle(Color c)
    {
       
        ParticleSystem.MainModule settings = psClick.main;
        settings.startColor = new ParticleSystem.MinMaxGradient(c);
        var psUI = psClick.gameObject.GetComponent<UIParticleSystem>();
        psUI.StartParticleEmission();

    }

    void FollowMouse()
    {
        transform.position = CanvasPointerPosition;
    }


    public BoardCell tutorialCell = null;
    private int indx = 0;
  
    public void OnPointerDown()
    {

        if (TutorialManager.I.IsTutorial) return;

        activeLine = null;

        BoardCell bc = raycaster.GetUIElement<BoardCell>();

        
       
        if (bc == null || bc.mLine == null) return;
        if (!InputManager.I.IsTouchActiveted) return;

        if (LevelManager.IsFirstLevel())
        {
            FirstLevelCondition(bc);
            if (bc != tutorialCell || bc != lastBoardSelected && bc.i != tutorialCell.i) return;
        }
        if (bc.IsHintCell) return;
        lastBoardSelected = bc;
        activeLine = bc.mLine;
       
        if (!GameInManager.I.GameInController.ConnectionController.IsLastCellOfLine(bc))
        {
            // öncekileri sil seçileni son yap
            activeLine.SetLastCell(bc,false);

            if(bc.mLine!=null && bc.mLine.connectedCell!=null)
            {
                activeLine.initCell.ResetWordCellText();
               
            }
            
        }

        activeLine.Draw(false);
        PlayParticle(activeLine.LineColor);
        bc.AnimateShake();

    }


    private void FirstLevelCondition(BoardCell bc)
    {
        BoardCell _targetCell;
        _targetCell = GameInManager.I.GameInController.ConnectionController.lsConnectNumberCells[indx];
        if (tutorialCell == null)
        {

            if (bc.IsNumber())
            {

                if (_targetCell == bc)
                {
                    tutorialCell = bc;
                    bc.isFirstLevelCondition = true;

                }
                else
                {
                    return;
                }
            }
            else if (bc == lastBoardSelected && bc.i == _targetCell.i)
            {

                tutorialCell = bc;
                bc.isFirstLevelCondition = true;
            }
            else { return; }


        }
        else
        {

            if (bc == lastBoardSelected && bc.i == _targetCell.i)
            {
                tutorialCell = bc;
                bc.isFirstLevelCondition = true;
            }
        }

    }
    public void OnDrag()
    {
        
        if (activeLine != null)
        {
            BoardCell bc = raycaster.GetUIElement<BoardCell>();
           
            if (bc == null) return;

            if (lastBoardSelected.IsWord()) return;

            if (!lastBoardSelected.IsNeighbourOf(bc)) return;

            // YENİ BOARDA GİRDİK
            if (bc != lastBoardSelected)
            {
               
                // bu cellden başka line geçiyor mu
                if (bc.mLine != null)
                {
                    if (bc.IsHintCell) return;

                    if (bc.IsNumber())
                    {
                        return;
                    }

                    if (bc.IsWord())
                    {
                       
                        bc.mLine.initCell.ResetWordCellText();
                        bc.mLine.SetLastCell(bc, true);
                        activeLine.AddCell(bc);
                        Connection(bc, activeLine);
                       
                        //return;
                    }

                    // bu cellden geçen line şu an çizdiğim line mı
                    if (bc.mLine == activeLine)
                    {
                        activeLine.SetLastCell(bc, false);
                        lastBoardSelected = bc;
                    }
                    //diğer line ı kopart
                    else
                    {
                        if (LevelManager.IsFirstLevel())
                        { 
                            if(bc.mLine != activeLine)
                            {
                                return;
                            }
                        }
                        bc.mLine.initCell.ResetWordCellText();
                        bc.mLine.SetLastCell(bc, true);
                        activeLine.AddCell(bc);
                    }
                }
                else
                {
                    if (LevelManager.IsFirstLevel())
                    {
                        
                        bc.isFirstLevelCondition = true;

                    }

                    if (bc.IsNumber())
                    {
                        return;
                    }

                    activeLine.AddCell(bc);
                    lastBoardSelected = bc;
                   

                    if(!bc.IsEmpty())
                    {
                        SoundManager.I.PlayOneShot(Sounds.LineDraw, .1f);
                        SoundManager.I.SetPitch( 1 + ((activeLine.lsCells.Count-1) * .1f));
                    }

                    if (bc.IsWord())
                    {
                       
                        if (bc.HasLineOnMe())
                        {
                            bc.ResetWordCellText();
                        }

                        Connection(bc,activeLine);
                       


                        if (LevelManager.IsFirstLevel())
                        {
                            TutorialManager.I.SecondFingerCondition();
                            tutorialCell = null;
                            indx += 1;

                        }

                        return;
                    }
                }
            }
        }
       
    }

    public void OnPointerUp()
    {
        
    }

    public async void Connection(BoardCell bc,Lines line)
    {
       
        await BoardCellIsWordConnection(bc.VALUE, bc, line);
        
    }

    public async Task BoardCellIsWordConnection(int _value, BoardCell _bc, Lines line)
    {
        Debug.Log("ınputCondition");
        //InputManager.IsTouchActiveted = false;
        line.initCell.SetWordCellText(_value, _bc);
        line.initCell.ParticlePlay(line.initCell.mLine.LineColor);
        _bc.ParticlePlay(line.initCell.mLine.LineColor);
        _bc.AnimateShake();


        new DelayAction(() => { 

            EventManager.Send(OnCorrectControl.Create());
            
        }, 1.2f).Execute(this);
      
        line.connectedCell = _bc;
        line = null;
        await Task.CompletedTask;
    }

}

