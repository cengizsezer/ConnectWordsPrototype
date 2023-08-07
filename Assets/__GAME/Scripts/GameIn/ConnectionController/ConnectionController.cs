using DG.Tweening;
using Sirenix.OdinInspector;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UnityEngine;

public sealed class ConnectionController
{
    public List<Lines> lsLines;
    public List<BoardCell> lsFullCell;
    public List<BoardCell> lsConnectNumberCells ;
    public List<BoardCell> lsConnectWordCells;

    public Sprite RightLeftSprite;
    private RectTransform Parent;
    private RectTransform ribbon;

    public GeneratorController Controller;
  
    public ConnectionController(ConnectionControllerSceneReferences references)
    {
        RightLeftSprite = references.RightLeftSprite;
        Parent = references.Parent;
        ribbon = references.Ribbon;
        lsLines = new();
        lsConnectNumberCells = new();
        lsConnectWordCells = new();
        lsFullCell = new();


    }
    public void Init(List<BoardCell> lsNumberCell)
    {
        
        for (int i = 0; i < lsNumberCell.Count; i++)
        {
            

            lsConnectNumberCells.Add(lsNumberCell[i]);
            Lines lines = new(lsNumberCell[i]);
            lsLines.Add(lines);
               
                lsNumberCell[i].mLine = lines;
            lines.LineColor = lsNumberCell[i].img.color;

            for (int a = 0; a < lsNumberCell[i].myHintLines.lsHintCells.Count; a++)
            {
                var prevCell = lsNumberCell[i].myHintLines.lsHintCells[a];
                lines.lsHintLinesCells.Add(prevCell);
            }

          
        }
    }
    public (BoardCell,BoardCell) MakePromter()
    {
        for (int i = 0; i < lsConnectNumberCells.Count; i++)
        {
            for (int a = 0; a < lsConnectWordCells.Count; a++)
            {
                if(lsConnectNumberCells[i].CorrectWord==lsConnectWordCells[a].VALUE)
                {
                    return (lsConnectNumberCells[i], lsConnectWordCells[a]);
                }
            }
        }

        return (null, null);
    }
    public int SearchWrongCondationIndex()
    {
        var lsPrev = lsLines.Where(n=>n.connectedCell != null).ToList();

        Board board= Controller.BoardGenerator.GetBoard();
        

        var WrongCondation = lsPrev.Where(n=> n.connectedCell.VALUE != n.initCell.CorrectWord).FirstOrDefault();

       
        if (WrongCondation != null)
        {
            Debug.Log(WrongCondation + "initcellVALUE-----" + WrongCondation.initCell.CorrectWord + "ConnectedcellVALUE-----" + board.GetCorrectValue(WrongCondation.connectedCell.VALUE));

            return lsLines.IndexOf(WrongCondation);
        } 

        return -1;
     
    }
    public void ResetLine(Lines line)
    {
        line.SetLastCell(line.initCell,false);
    }
    public async Task SetWordCells(List<BoardCell> lsNumberCell,List<BoardCell> lsWordCell)
    {

      

        while (Controller.WordGenerator.GetWordTable().activeCells.Count == 0)
        {
            await Task.Yield();
        }

        for (int i = 0; i < lsNumberCell.Count; i++)
        {

            for (int a = 0; a < Controller.WordGenerator.GetWordTable().activeCells.Count; a++)
            {
                WordTableCell wtCell = Controller.WordGenerator.GetWordTable().activeCells[a];
               
                if (lsNumberCell[i].VALUE == wtCell.VALUE)
                {
                    
                    lsNumberCell[i].lsWhichWordToConnect.Add(wtCell);
                }
            }
        }


        for (int i = 0; i < lsWordCell.Count; i++)
        {
           
            lsConnectWordCells.Add(lsWordCell[i]);
        }

       await Task.Yield();
    }
    public void SetFullCell()
    {
        for (int i = 0; i < lsLines.Count; i++)
        {
            for (int a = 0; a < lsLines[i].lsCells.Count; a++)
            {
                if(!lsFullCell.Contains(lsLines[i].lsCells[a]))
                {
                    lsFullCell.Add(lsLines[i].lsCells[a]);
                }
               
            }
        }
    }
    public async Task LevelEndCellAnimate()
    {
        if (LevelManager.IsFirstLevel()&&TutorialManager.I.ActiveFinger!=null)
        {
            TutorialManager.I.ActiveFinger.gameObject.SetActive(false);
        }

        //Debug.Log(System.Threading.Thread.CurrentThread.ManagedThreadId + ".Thread" + "islem" + "-----3");
        InputManager.I.IsTouchActiveted = false;
        var lsLinesParent = GetParentList().ToList();
        var lsSuffle = GetSuffle(lsLinesParent).ToList();
        await LevelEndEmptyCellMoving(lsSuffle, lsLinesParent); 

    }
   
    
    
    public IEnumerable<GameObject> GetParentList()
    {
        List<GameObject> lsLinesParent = new List<GameObject>();

        for (int i = 0; i < lsLines.Count; i++)
        {
            GameObject prevParent = new GameObject("prevParent", typeof(RectTransform));
            lsLinesParent.Add(prevParent);
            prevParent.transform.SetParent(Parent.transform);

            for (int a = 0; a < lsLines[i].lsCells.Count; a++)
            {
                lsLines[i].lsCells[a].transform.SetParent(prevParent.transform);
            }
        }

        return lsLinesParent;
    }
    public IEnumerable<GameObject> GetSuffle(List<GameObject> lsLinesParent)
    {
        System.Random rnd = new System.Random();
        var lsSuffle = lsLinesParent.Select(x => new { value = x, order = rnd.Next() })
           .OrderBy(x => x.order).Select(x => x.value).ToList();

        return lsSuffle;
    }
    public async Task LevelEndEmptyCellMoving(List<GameObject> lsSuffle,List<GameObject> lsLinesParent)
    {

        var lsPrev = GetEmptyCell();
        var lsSelect = lsPrev.Where(n => n).Reverse().ToList();
        var tasks = new List<Task>();

        for (int i = 0; i < lsSelect.Count; i++)
        {
            tasks.Add(lsSelect[i].AnimateEmptyCell());
        }

        await Task.WhenAll(tasks);
        tasks.Clear();

        GameObject prevParent = new GameObject("BG", typeof(RectTransform));
        prevParent.transform.SetParent(Parent.transform);

        List<Tween> lsTween = new();
        SoundManager.I.PlayOneShot(Sounds.LevelSuccess);
        for (int i = 0; i < lsSuffle.Count; i++)
        {
            Debug.Log(lsSuffle[i]);
           Tween t=TweenHelper.ShakePosition(lsSuffle[i].transform, null, 20, 1f);
            lsTween.Add(t);
        }

       

        while (lsTween.TrueForAll(n=>n.IsActive()))
        {
            
            await Task.Yield();
        }

        for (int i = 0; i < lsSuffle.Count; i++)
        {
            lsSuffle[i].transform.SetParent(prevParent.transform);
        }
        await GameInUIManager.I.FadeShadow();

       


        Sequence boardCellSeq = DOTween.Sequence();
        boardCellSeq.Append(TweenHelper.ShrinkDisappear(prevParent.transform, null, 1f));
        boardCellSeq.AppendInterval(0.2f);
        for (int i = 0; i < lsFullCell.Count; i++)
        {
            boardCellSeq.Join(lsFullCell[i].lineImg.DOFade(0f, 1f).From(1f));
            boardCellSeq.Join(lsFullCell[i].img.DOFade(0f, 1f).From(1f));
            boardCellSeq.Join(lsFullCell[i].txt.DOFade(0f, 1f).From(1f));
        }

        Sequence ribbonSeq = DOTween.Sequence();
        

        Sequence wordCellSeq = DOTween.Sequence();
        WordTable wt = Controller.WordGenerator.crntWordTable;

        
        for (int i = 0; i < wt.activeCells.Count; i++)
        {
            wordCellSeq.Join(TweenHelper.ShrinkDisappear(wt.activeCells[i].transform, null, 1f));
        }

        Vector2 ribbonInitialPos = ribbon.anchoredPosition;
        ribbonInitialPos.y += 570 + 4.334961f;
        wordCellSeq.Join(ribbon.DOAnchorPos(ribbonInitialPos, .7f).SetDelay(.4f));
        wordCellSeq.Join(ribbon.DOScale(1f, .7f));


        wordCellSeq.AppendInterval(0.2f);
        for (int i = 0; i < wt.activeCells.Count; i++)
        {

            wordCellSeq.Join(wt.activeCells[i].img.DOFade(0f, 1f).From(1f));
            wordCellSeq.Join(wt.activeCells[i].txt.DOFade(0f, 1f).From(1f));
        }


       


    }
    public List<BoardCell> GetEmptyCell()
    {
        
        var lsprev = Controller.BoardGenerator.GetBoard().lsAllCells;

        return lsprev.Except(lsFullCell).ToList();

    }
    public Lines GetLine(BoardCell bc)
    {
        foreach (var item in lsLines)
        {
            if(item.lsCells.Contains(bc))
            {
                return item;
            }
        }

        return null;
    }
    public int GetLineIndex(BoardCell bc)
    {
        foreach (var item in lsLines)
        {
            if (item.lsCells.Contains(bc))
            {
                return item.lsCells.IndexOf(bc);
            }
        }

        return -1;
    }
    public bool IsLastCellOfLine(BoardCell bc)
    {

        foreach (var item in lsLines)
        {
            if (item.lsCells.Contains(bc))
            {
                return item.lsCells[^1] == bc;
            }
        }

        return false;
    }
    public void CreateFlyingSpriteToGoal(BoardCell entity, WordTableCell target,Action OnComplote=null)
    {
        char mChar = System.Convert.ToChar(entity.VALUE);

        UIEffectsManager.I.CreateCurvyFlyingSprite(
            entity,
            entity.img.sprite,
            entity.mLine.LineColor,
            mChar.ToString(),
            entity.GetComponent<RectTransform>().sizeDelta * 1.25f, // create bigger flying image for better visual representation
            entity.transform.position,
            target.transform.position,
            0.5f,
            UIEffectsManager.CanvasLayer.OverEverything,
            () =>
            { 
                target.ParticlePlay(WordParticleType.connection, entity.mLine.LineColor);
                OnComplote?.Invoke();
            });
    }
    
}


