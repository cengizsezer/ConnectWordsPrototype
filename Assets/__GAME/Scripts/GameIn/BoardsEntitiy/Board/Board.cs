using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UnityEngine;
using MyProject.Core.Manager;
using MyProject.Extentions;


[System.Serializable]
public class Board : GenericBoardBase<BoardCell>, IBoard
{

    public List<BoardCell> activeCells { get; set; }
    public List<BoardCell> lsAllCells { get; set; }
    public List<BoardCell> lsNumberCell { get; set; }
    public List<BoardCell> lsWordCell { get; set; }
    public List<HintController> lsHintCells { get; set; }

    public List<PrevPairs> lsPrev = new();
    public List<CorrectPairs> lsCorrect = new();
   
    private Dictionary<int, int> correctPairs;
   
    private Dictionary<int, int> newDic = new Dictionary<int, int>();

    private LevelManager.Level.BoardInfo mInfo;
    public LevelManager.Level.CellInfo[] arrNewAvailableCells;
    int yCount, xCount;
  
    [Serializable]
    public class PrevPairs
    {
        public int key = 0;
        public int value = 0;
    }
    [Serializable]
    public class CorrectPairs
    {
        public int key = 0;
        public int value = 0;
    }
    public Board(LevelManager.Level.BoardInfo bInfo)
    {
        mInfo = bInfo;
        activeCells = new List<BoardCell>();
        lsNumberCell = new List<BoardCell>();
        lsAllCells = new List<BoardCell>();
        lsWordCell = new List<BoardCell>();
        lsHintCells = new List<HintController>();

        xCount = bInfo.columnCount;
        yCount = bInfo.rowCount;

        cellEmptyImageColor = ToColorFromHex(bInfo.cellColors.hexColorEmptyImage);
        cellEmptyTextColor = ToColorFromHex(bInfo.cellColors.hexColorEmptyText);
        cells = new BoardCell[bInfo.columnCount, bInfo.rowCount];
       
    }
    public override async Task FillImagePairs()
    {
        correctPairs = new Dictionary<int, int>();
        //colorPairs = new Dictionary<int, Color>();
        lsColorPairs = new List<ColorPairs>();

        for (int i = 0; i < mInfo.correctPairs.Length; i++)
        {
            //Debug.Log("MINFO CORRECT PAIRS _  " + i + " = " + mInfo.correctPairs[i].intID + " -> " + mInfo.correctPairs[i].charID);

            if (!correctPairs.ContainsKey(mInfo.correctPairs[i].intID))
            {
                correctPairs.Add(mInfo.correctPairs[i].intID, mInfo.correctPairs[i].charID);
            }
            Color c = Color.red;
            if (!colorPairs.ContainsKey(mInfo.correctPairs[i].intID))
            {
                //c = mInfo.cellColors.lineColors[i].ToColorFromHex();
                c = ToColorFromHex(mInfo.cellColors.lineColors[i]);
                c.SetAlpha(.95f);
                Color _c = new Color(c.r, c.g, c.b, .99f);
                ColorPairs cp = new ColorPairs();
                //cp.id = mInfo.correctPairs[i].intID;
                cp.id = System.Convert.ToChar(mInfo.correctPairs[i].intID);
                cp.color = _c;
                lsColorPairs.Add(cp);
                colorPairs.Add(mInfo.correctPairs[i].intID, _c);
            }
        }

        await Task.Yield();
    }
    public override async Task FillTextPairs()
    {

        lsTextColors = new List<ColorPairs>();

        for (int i = 0; i < mInfo.cellColors.textColors.Length; i++)
        {
            Color c = Color.red;
            c = ToColorFromHex(mInfo.cellColors.textColors[i]);
            ColorPairs cp = new ColorPairs();
            cp.color = c;
            cp.id = System.Convert.ToChar(mInfo.cellColors.colorNumbers[i]);
            lsTextColors.Add(cp);

        }
        await Task.Yield();
    }
    public override async Task CreateBoardCells()
    {

        for (int i = 0; i < xCount; i++)//3
        {
            for (int j = 0; j < yCount; j++)//2
            {
                BoardCell _bc = PoolHandler.I.GetObject<BoardCell>();
                _bc.BoardGenerator = BoardGenerator;
                _bc.transform.SetParent(_bc.BoardGenerator.GetPoolParent());
                cells[i, j] = _bc;
                _bc.i = i;
                _bc.j = j;
#if UNITY_EDITOR
                _bc.gameObject.name = _bc.i + "," + _bc.j;
#endif
                _bc.VALUE = 0;
                lsAllCells.Add(_bc);

            }
        }

        await Task.Yield();
    }
    public override async Task SetCellsAvailable()
    {

        for (int i = 0; i < arrNewAvailableCells.Length; i++)
        {
            cells[arrNewAvailableCells[i].i, arrNewAvailableCells[i].j].VALUE = arrNewAvailableCells[i].ASCII;

            if (cells[arrNewAvailableCells[i].i, arrNewAvailableCells[i].j].IsNumber())
            {
                lsNumberCell.Add(cells[arrNewAvailableCells[i].i, arrNewAvailableCells[i].j]);
            }
            else if (cells[arrNewAvailableCells[i].i, arrNewAvailableCells[i].j].IsWord())
            {
                lsWordCell.Add(cells[arrNewAvailableCells[i].i, arrNewAvailableCells[i].j]);
            }

            cells[arrNewAvailableCells[i].i, arrNewAvailableCells[i].j].SetAvailable(true);
            activeCells.Add(cells[arrNewAvailableCells[i].i, arrNewAvailableCells[i].j]);

        }

        for (int i = 0; i < lsNumberCell.Count; i++)
        {
            HintController hintController = new HintController();

            for (int a = 0; a < mInfo.hintCells[i].cell.Length; a++)
            {
                int prevI = mInfo.hintCells[i].cell[a].i;
                int prevJ = mInfo.hintCells[i].cell[a].j;
                hintController.AddHintCells(cells[prevI, prevJ]);
            }

            lsHintCells.Add(hintController);

        }

        for (int i = 0; i < lsNumberCell.Count; i++)
        {
            for (int a = 0; a < lsHintCells.Count; a++)
            {
                if (cells[lsNumberCell[i].i, lsNumberCell[i].j] == cells[lsHintCells[a].lsHintCells.FirstOrDefault().i, lsHintCells[a].lsHintCells.FirstOrDefault().j])
                {
                    lsNumberCell[i].myHintLines = lsHintCells[a];

                }
            }

            //lsNumberCell[i].CorrectWord = GetCorrectValue(lsNumberCell[i].VALUE);
            //lsNumberCell[i].CorrectWord = mInfo.correctPairs[i].charID;
        }

        SetCorrectValues(lsNumberCell);
        GameInManager.I.GameInController.ConnectionController.Init(lsNumberCell);

       
        while (WordTableGenerator.crntWordTable.activeCells.Count < 0)
        {
            await Task.Yield();
        }

        await GameInManager.I.GameInController.ConnectionController.SetWordCells(lsNumberCell, lsWordCell);
        TutorialManager.I.AddBackGroundList(lsNumberCell, lsWordCell);

        await Task.Yield();
    }
    public override async void Spawn()
    {
        
        
        var signal = new TaskCompletionSource<bool>();
        //WordTableGenerator wtg = PoolHandler.I.wordTableGenerator;
        Task TaskGroup1 = new Task(async () =>
        {
            await CreateBoardCells();
            await FillImagePairs();
            await FillTextPairs();


            if (SaveLoadManager.GetMaxTotalLevel() >= MyProject.Constant.Constants.BoardSettings.mixAfter)
            {
                while (!WordTableGenerator.GetWordTable().hasMixed)
                {
                    await Task.Yield();
                }
                await MixBoard(WordTableGenerator.GetWordTable().dic_asciiPairs);

            }
            else
            {
                arrNewAvailableCells = mInfo.availableCells;
            }

            //SetCorrectValues(lsNumberCell);

            await SetCellsAvailable();
            signal.SetResult(true);

        });

        TaskGroup1.Start(TaskScheduler.FromCurrentSynchronizationContext());
        await TaskGroup1;
        await signal.Task;
        
        BoardGenerator.EnableGridLayout(false);


    }

    public async Task MixBoard(Dictionary<int, int> asciiPairs)
    {
        foreach (var _cPairs in correctPairs)
        {
            var correct = new CorrectPairs();
            correct.key = _cPairs.Key;
            correct.value = _cPairs.Value;
            lsCorrect.Add(correct);
        }

        arrNewAvailableCells = new LevelManager.Level.CellInfo[mInfo.availableCells.Length];

        for (int i = 0; i < mInfo.availableCells.Length; i++)
        {
            arrNewAvailableCells[i] = new LevelManager.Level.CellInfo();
            arrNewAvailableCells[i].i = mInfo.availableCells[i].i;
            arrNewAvailableCells[i].j = mInfo.availableCells[i].j;
            arrNewAvailableCells[i].ASCII = mInfo.availableCells[i].ASCII;
        }

        for (int i = 0; i < arrNewAvailableCells.Length; i++)
        {
            if (asciiPairs.TryGetValue(arrNewAvailableCells[i].ASCII, out int _i))
            {
                //arrNewAvailableCells[i] = arrNewAvailableCells[i];
                arrNewAvailableCells[i].ASCII = _i;
            }
        }
       
        foreach (var _cPairs in correctPairs)
        {

            int val = _cPairs.Value;//value sabit

            if (asciiPairs.TryGetValue(_cPairs.Key, out int newKey))//correctPairsın keyini aratıp yeni keyi al
            {
                newDic.Add(newKey, val);//yeni dictonarye ekle

                var prev = new PrevPairs();
                prev.key = newKey;
                prev.value = val;
                lsPrev.Add(prev);
            }
        }

        correctPairs = newDic;

        await Task.Yield();


    }
    public void SetCorrectValues(List<BoardCell> lsNumberCell)
    {

        for (int i = 0; i < lsNumberCell.Count; i++)
        {
            if (correctPairs.TryGetValue(lsNumberCell[i].VALUE, out int val))
            {
                lsNumberCell[i].CorrectWord = val;
            }
        }
    }
    public int GetCorrectValue(int id)
    {
        int value = -1;

        if (correctPairs.TryGetValue(id, out int val))
        {

            value = val;

            return value;
        }

        return value;
    }
   

}