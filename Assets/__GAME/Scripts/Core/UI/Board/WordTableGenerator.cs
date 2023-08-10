using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using MyProject.Settings;
using System.Threading.Tasks;
using System.Linq;
using System;

public class WordTableGenerator : GeneratorBase<WordTableGenerator>, IDisposable
{
    public WordTable crntWordTable;
    public WordTable GetWordTable() => crntWordTable;
    public GeneratorController Controller;
    public List<RowController> lsRows;
    public List<Color> lsWordColors;
    public List<WordTableCell> lsWordTableCells;
    public float ClampValue;
    public float CellScaleOffSet;

    private int childOrderValue = 1;
    protected override int ChildOrder
    {
        get { return childOrderValue; }
        set { childOrderValue = value; }
    }
   

    public WordTableGenerator()
    {
        AddGeneratorToOrderList(this, childOrderValue);
    }

    public WordTableGenerator(WordTableGeneratorSettings settings, WordTableGeneratorReferences references)
    {

        lsRows = new();
        lsWordColors = new();
        lsWordTableCells = new();
        rectTransform = references.RectTransform;
        gridLayoutGroup = references.GridLayoutGroup;

        CellScaleOffSet = settings.CellScaleOffSet;
        ClampValue = settings.ClampValue;

        AddGeneratorToOrderList(this, childOrderValue);
       
    }

    public override async Task CreateBoardAsync()
    {
        await Init();
    }

    private async Task Init()
    {
        
        InputManager.I.IsTouchActiveted = false;
        var vInfo = LevelManager.GetCurrentLevel().wordTable;
        EventManager.RegisterHandler<TutorialSwipeEvent>(OnTutorialSwipeEvent);
        cellCount_Horizontal = vInfo.columnCount;
        cellCount_Vertical = vInfo.rowCount;

        AdjustScale();

        crntWordTable = new WordTable(vInfo);
        crntWordTable.WordTableGenerator = this;
        crntWordTable.BoardGenerator = Controller.BoardGenerator;
        SoundManager.I.PlayOneShot(Sounds.Transition, 1f);
        crntWordTable.Spawn();
        EventManager.RegisterHandler<OnCorrectControl>(OnCorrectControl);
        await Task.CompletedTask;
    }


    private void OnTutorialSwipeEvent(TutorialSwipeEvent @event)
    {
        TutorialManager.I.CreateBGObject(rectTransform, GameInUIManager.I.PanelBoard.transform);
    }

    public async void OnCorrectControl(OnCorrectControl c)
    {
       
        bool IsCorrect = lsRows.TrueForAll(n=>n.IsCorrect());
        bool IsFill = lsRows.TrueForAll(n=>n.FillControl());
        Debug.Log(IsCorrect);
        Debug.Log(IsFill);

        if (IsFill && !IsCorrect)
        {
           
            EventManager.Send(OnWrongFeedBack.Create());
            await Task.Yield();
            return;
        }

        if (IsFill && IsCorrect && GameManager.I.isRunning)
        {
            GameManager.I.isRunning = false;
            GameInManager.I.GameInController.ConnectionController.SetFullCell();
            var ls = GameInManager.I.GameInController.ConnectionController.GetEmptyCell();
            //Debug.Log(System.Threading.Thread.CurrentThread.ManagedThreadId + ".Thread" + "islem" + "-----1");

            await GameInManager.I.GameInController.ConnectionController.LevelEndCellAnimate()
                .ContinueWith(async _ =>
                {
                    await Task.Delay(1000);
                    await GameManager.I.OnLevelSuccess();
                }, TaskScheduler.FromCurrentSynchronizationContext());

            //await MainThreadDispatcher.Instance.ExecuteOnMainThreadAsync(async () =>
            //{
            //    await Task.Delay(1000);
            //    await GameManager.I.OnLevelSuccess();
            //});
        }
       
    }
    
    protected override void AdjustScale()
    {
        var vInfo = LevelManager.GetCurrentLevel().wordTable;
         //get rectSize according to current screen sizes
         Vector2 rectSize = rectTransform.rect.size;
        float minSizeOfRect = Mathf.Min(rectSize.x, rectSize.y);
        rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, 500f);
        rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, 500f);

        rectSize = Vector2.one * minSizeOfRect;
        SettingsSO sso = Settings.GetSettings();

        //calculate paddings according to Settings and Set the value
        if (sso.BoardPaddings.sizingType == Sizing.FixedPixel)
        {
            gridLayoutGroup.padding.bottom = sso.BoardPaddings.pixels.bottom;
            gridLayoutGroup.padding.top = sso.BoardPaddings.pixels.top;
            gridLayoutGroup.padding.left = sso.BoardPaddings.pixels.left;
            gridLayoutGroup.padding.right = sso.BoardPaddings.pixels.right;
        }
        else
        {
            int _bottom = Mathf.FloorToInt(rectSize.y * sso.BoardPaddings.percentages.bottom);
            int _top = Mathf.FloorToInt(rectSize.y * sso.BoardPaddings.percentages.top);
            int _right = Mathf.FloorToInt(rectSize.x * sso.BoardPaddings.percentages.right);
            int _left = Mathf.FloorToInt(rectSize.x * sso.BoardPaddings.percentages.left);

            gridLayoutGroup.padding.bottom = _bottom;
            gridLayoutGroup.padding.top = _top;
            gridLayoutGroup.padding.left = _left;
            gridLayoutGroup.padding.right = _right;
        }

        //calculate cell size according to Settings and Set the value
        if (sso.CellSpaces.sizingType == Sizing.FixedPixel)
        {
            gridLayoutGroup.spacing = sso.CellSpaces.pixels.space;
        }
        else if (sso.CellSpaces.sizingType == Sizing.Percentage)
        {
            float _x = rectSize.x - (gridLayoutGroup.padding.left + gridLayoutGroup.padding.right);
            _x = _x * sso.CellSpaces.percentages.space.x;
            _x /= (vInfo.rowCount - 1);

            float _y = rectSize.y - (gridLayoutGroup.padding.bottom + gridLayoutGroup.padding.top);
            _y = _y * sso.CellSpaces.percentages.space.y;
            _y /= (vInfo.columnCount - 1);

            if (sso.CellSpaces.setEqualXY)
            {
                switch (sso.CellSpaces.setTo)
                {
                    case SetType.Min:
                        float min = Mathf.Min(_x, _y);
                        gridLayoutGroup.spacing = Vector2.one * min;
                        break;
                    case SetType.Max:
                        float max = Mathf.Max(_x, _y);
                        gridLayoutGroup.spacing = Vector2.one * max;
                        break;
                    case SetType.Average:
                        float average = (_x + _y) / 2f;
                        gridLayoutGroup.spacing = Vector2.one * average;
                        break;
                }
            }
            else
            {
                gridLayoutGroup.spacing = new Vector2(_x, _y);
            }
        }

        //find optimum vertical and horizontal size
        float verticalCellSize = (rectSize.y - (gridLayoutGroup.padding.bottom + gridLayoutGroup.padding.top + (sso.CellSpaces.pixels.space.y * vInfo.columnCount))) / cellCount_Vertical;
        float horizontalCellSize = (rectSize.x - (gridLayoutGroup.padding.right + gridLayoutGroup.padding.left) + (sso.CellSpaces.pixels.space.x * vInfo.rowCount)) / cellCount_Horizontal;

        //find minimum cell size so both heigth and width fits
        cellSize = Mathf.Min(Mathf.Clamp(verticalCellSize,0f, ClampValue), Mathf.Clamp(horizontalCellSize, 0f, ClampValue));

        //adjust the gridLayoutGroup accordint to calculated cell size
        gridLayoutGroup.constraint = GridLayoutGroup.Constraint.FixedRowCount;
        gridLayoutGroup.constraintCount = cellCount_Horizontal;
        gridLayoutGroup.cellSize = (Vector2.one * cellSize) + new Vector2(CellScaleOffSet, CellScaleOffSet);

    }

    public void Dispose()
    {
        lsRows.Clear();
        lsWordColors.Clear();
        lsWordTableCells.Clear();


        lsRows = null;
        lsWordColors = null;
        lsWordTableCells = null;

        EventManager.UnregisterHandler<OnCorrectControl>(OnCorrectControl);
        EventManager.UnregisterHandler<TutorialSwipeEvent>(OnTutorialSwipeEvent);
    }
}
