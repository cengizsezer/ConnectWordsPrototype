using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using MyProject.Settings;
using System.Text;
using System.Threading.Tasks;

public class BoardGenerator : GeneratorBase<BoardGenerator>
{
    [SerializeField] RectTransform GridHolder;
    [SerializeField] float GridHolderOffSet;
    public GeneratorController Controller;
    public Board crntBoard;
    public Board GetBoard() => crntBoard;
    public float ClampValue;

    private int childOrderValue = 10;
    protected override int ChildOrder
    {
        get { return childOrderValue; }
        set { childOrderValue = value; }
    }
    public BoardGenerator() : this(null,null)
    {
        
    }

    public BoardGenerator(BoardGeneratorSettings settings,BoardGeneratorSceneReferences references)
    {
       
        GridHolder = references.GridHolder;
        rectTransform = references.RectTransform;
        gridLayoutGroup = references.GridLayoutGroup;

        GridHolderOffSet = settings.GridHolderOffSet;
        ClampValue = settings.ClampValue;

        AddGeneratorToOrderList(this, childOrderValue);
    }

    public override async Task CreateBoardAsync()
    {
        await Init();
    }

    private async Task Init()
    {
       
        var bInfo = LevelManager.GetCurrentLevel().board;
        EventManager.RegisterHandler<TutorialSwipeEvent>(OnTutorialSwipeEvent);

        cellCount_Horizontal = bInfo.columnCount;//i
        cellCount_Vertical = bInfo.rowCount;//j

        AdjustScale();

        crntBoard = new Board(bInfo);
        crntBoard.BoardGenerator = this;
       
        crntBoard.WordTableGenerator = Controller.WordGenerator;
        crntBoard.Spawn();

        if (LevelManager.IsFirstLevel())
        {
            Debug.Log("firstlevel");
            EventManager.Send(TutorialSwipeEvent.Create());
            TutorialManager.I.UIElementActiveted(false);

        }

        await Task.Yield();
    }
    private void OnTutorialSwipeEvent(TutorialSwipeEvent @event)
    {
        TutorialManager.I.IsTutorial = true;
        TutorialManager.I.CreateBGObject(rectTransform,GameInUIManager.I.PanelBoard.transform);
    }
    protected override void AdjustScale()
    {
        var bInfo = LevelManager.GetCurrentLevel().board;
        //get rectSize according to current screen sizes
        Vector2 rectSize = GridHolder.rect.size;
        float minSizeOfRect = Mathf.Min(rectSize.x, rectSize.y);
        GridHolder.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, minSizeOfRect);
        GridHolder.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, minSizeOfRect);

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
        if(sso.CellSpaces.sizingType == Sizing.FixedPixel)
        {
            gridLayoutGroup.spacing = sso.CellSpaces.pixels.space;
        }
        else if(sso.CellSpaces.sizingType == Sizing.Percentage)
        {
            float _x = rectSize.x - (gridLayoutGroup.padding.left + gridLayoutGroup.padding.right);
            _x = _x * sso.CellSpaces.percentages.space.x;
            _x /= (bInfo.rowCount - 1);

            float _y = rectSize.y - (gridLayoutGroup.padding.bottom + gridLayoutGroup.padding.top);
            _y = _y * sso.CellSpaces.percentages.space.y;
            _y /= (bInfo.columnCount - 1);

            if(sso.CellSpaces.setEqualXY)
            {
                switch(sso.CellSpaces.setTo)
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
        float verticalCellSize = (rectSize.y - (gridLayoutGroup.padding.bottom + gridLayoutGroup.padding.top + (sso.CellSpaces.pixels.space.y * bInfo.columnCount))) / cellCount_Vertical;
        float horizontalCellSize = (rectSize.x - (gridLayoutGroup.padding.right + gridLayoutGroup.padding.left) + (sso.CellSpaces.pixels.space.x * bInfo.rowCount)) / cellCount_Horizontal;

        //find minimum cell size so both heigth and width fits
        cellSize = Mathf.Min(Mathf.Clamp(verticalCellSize,0f, ClampValue), Mathf.Clamp(horizontalCellSize, 0f, ClampValue));

        //adjust the gridLayoutGroup accordint to calculated cell size
        gridLayoutGroup.constraint = GridLayoutGroup.Constraint.FixedRowCount;
        gridLayoutGroup.constraintCount = cellCount_Horizontal;
        gridLayoutGroup.cellSize = Vector2.one * cellSize;

        float _xAnchoredSize = (cellSize * cellCount_Vertical) + (gridLayoutGroup.spacing.x * (cellCount_Vertical + 1));
        float _yAnchoredSize = (cellSize * cellCount_Horizontal) + (gridLayoutGroup.spacing.y * (cellCount_Horizontal + 1));

        //Debug.Log("Cell Size : " + cellSize + " , Vertical Count : " + cellCount_Vertical + " , Horizontal Count : " + cellCount_Horizontal + " , X ==> " + _xAnchoredSize + ", Y ==> " + _yAnchoredSize);

        //GridHolder.rect.Set(0, 0, _xAnchoredSize, _yAnchoredSize);
        rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, _xAnchoredSize);
        rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, _yAnchoredSize);

        //GridHolder.sizeDelta = new Vector2(_xAnchoredSize, _yAnchoredSize);

    }
    private void OnDestroy()
    {
        EventManager.UnregisterHandler<TutorialSwipeEvent>(OnTutorialSwipeEvent);
    }


}
