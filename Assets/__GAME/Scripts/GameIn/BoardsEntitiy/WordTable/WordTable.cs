using DG.Tweening;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UnityEngine;
using MyProject.Core.Manager;

[System.Serializable]
public class WordTable:GenericBoardBase<WordTableCell>,IWordTable
{
   
    public List<WordTableCell> activeCells;

    public LevelManager.Level.WordTableInfo mInfo { get; set; }
    public LevelManager.Level.RowInfo rInfo { get; set; }
    public bool hasMixed { get; set; } = false;

    [System.Serializable]
    public class WordTableAscii
    {
        public int key = 0;
        public int value = 0;

    }

    public Dictionary<int, int> dic_asciiPairs = new();

    public List<WordTableAscii> lsWordTableAscii = new();

    public LevelManager.Level.WordCellInfo[] arrNewAvailableWordCells;

   
    public WordTable(LevelManager.Level.WordTableInfo bInfo)
    {
        mInfo = bInfo;
        rInfo = LevelManager.GetCurrentLevel().row;
        activeCells = new List<WordTableCell>();
        lineCount = bInfo.columnCount;
        rowCount = bInfo.rowCount;
        cellEmptyImageColor = ToColorFromHex(bInfo.wordCellColors.hexColorEmptyImage);
        cellEmptyTextColor = ToColorFromHex(bInfo.wordCellColors.hexColorEmptyText);
        //FillImagePairs();
        //FillTextPairs();
        cells = new WordTableCell[bInfo.columnCount, bInfo.rowCount];


    }

    public override async Task FillTextPairs()
    {

        lsTextColors = new List<ColorPairs>();

        for (int i = 0; i < mInfo.wordCellColors.textColors.Length; i++)
        {
            Color c = Color.red;
            c = ToColorFromHex(mInfo.wordCellColors.textColors[i]);
            ColorPairs cp = new ColorPairs();
            cp.color = c;
            cp.id = System.Convert.ToChar(mInfo.wordCellColors.colorNumbers[i]);
            lsTextColors.Add(cp);
            WordTableGenerator.lsWordColors.Add(c);
        }

        await Task.Yield();

    }

    public override async Task FillImagePairs()
    {

        lsColorPairs = new List<ColorPairs>();

        for (int i = 0; i < mInfo.wordCellColors.colors.Length; i++)
        {
            Color c = Color.red;
            c = ToColorFromHex(mInfo.wordCellColors.colors[i]);
            ColorPairs cp = new ColorPairs();
            cp.color = c;
            cp.id = System.Convert.ToChar(mInfo.wordCellColors.colorNumbers[i]);
            lsColorPairs.Add(cp);
            WordTableGenerator.lsWordColors.Add(c);
        }


        await Task.Yield();
    }
    public override async void Spawn()
    {

        var tcs = new TaskCompletionSource<bool>();
        await Task.Delay(500);

        if (SaveLoadManager.GetMaxTotalLevel() >= MyProject.Constant.Constants.BoardSettings.mixAfter)
            MixTable();
        else
            arrNewAvailableWordCells = mInfo.availableCells;

        Task TaskGroup1 = new Task(async () =>
        {
            await CreateBoardCells();
            await FillImagePairs();
            await FillTextPairs();
            await SetCellsAvailable();

            //await FadeAllWordTableCells();
            tcs.SetResult(true);

        });

        TaskGroup1.Start(TaskScheduler.FromCurrentSynchronizationContext());
        await TaskGroup1;

        await SetInitialPosWithY(tcs.Task);

        WordTableGenerator.HasInitialized = true;

    }
    public override async Task CreateBoardCells()
    {

        for (int i = 0; i < lineCount; i++)//3
        {
            for (int j = 0; j < rowCount; j++)//5
            {
                WordTableCell _wt = PoolHandler.I.GetObject<WordTableCell>();
                _wt.WordTableGenerator = WordTableGenerator;
                _wt.transform.SetParent(WordTableGenerator.GetPoolParent());
                cells[i, j] = _wt;
                _wt.i = i;
                _wt.j = j;
#if UNITY_EDITOR
                _wt.gameObject.name = _wt.i + "," + _wt.j;
#endif
                _wt.VALUE = 0;

            }
        }

        await Task.Yield();
    }
    public override async Task SetCellsAvailable()
    {
       
        for (int i = 0; i < arrNewAvailableWordCells.Length; i++)
        {
            cells[arrNewAvailableWordCells[i].i, arrNewAvailableWordCells[i].j].VALUE = arrNewAvailableWordCells[i].ASCII;
            cells[arrNewAvailableWordCells[i].i, arrNewAvailableWordCells[i].j].DefaultValue = arrNewAvailableWordCells[i].ASCII;
            cells[arrNewAvailableWordCells[i].i, arrNewAvailableWordCells[i].j].SetImageAlpha(0f);
            cells[arrNewAvailableWordCells[i].i, arrNewAvailableWordCells[i].j].SetTextAlpha(0f);
            cells[arrNewAvailableWordCells[i].i, arrNewAvailableWordCells[i].j].SetAvailable(true);
            //cells[mInfo.availableCells[i].i, mInfo.availableCells[i].j].SetText(cells[mInfo.availableCells[i].i, mInfo.availableCells[i].j].VALUE);
            activeCells.Add(cells[arrNewAvailableWordCells[i].i, arrNewAvailableWordCells[i].j]);

        }

        SetRows();

        await Task.Yield();
    }
    public void SetConnectionColors(WordTableCell wtc)
    {
        wtc.UpdateColor(WordTableGenerator.GetWordTable().GetEmptyImageColor());
        wtc.txt.color = WordTableGenerator.GetWordTable().GetEmptyTextColor();
    }
    public void MixTable()
    {
        dic_asciiPairs = new Dictionary<int, int>();
        Dictionary<int, int> asciiPairs = new Dictionary<int, int>();
        List<int> numbers = new List<int>();

        arrNewAvailableWordCells = mInfo.availableCells;
        

        for (int i = 0; i < arrNewAvailableWordCells.Length; i++)
        {
            if (!numbers.Contains(arrNewAvailableWordCells[i].ASCII))
            {
                numbers.Add(arrNewAvailableWordCells[i].ASCII);
            }

        }

        List<int> tmpCopy = new List<int>();
        tmpCopy.AddRange(numbers);

        for (int i = 0; i < numbers.Count; i++)
        {
            int newVal = tmpCopy[Random.Range(0, tmpCopy.Count)];
            asciiPairs.Add(numbers[i], newVal);
            tmpCopy.Remove(newVal);


            var tmp = new WordTableAscii();
            tmp.key = numbers[i];
            tmp.value = newVal;
            lsWordTableAscii.Add(tmp);

        }


        foreach (LevelManager.Level.WordCellInfo wci in arrNewAvailableWordCells)
        {
            if (asciiPairs.TryGetValue(wci.ASCII, out int _i))
            {
                wci.ASCII = _i;
            }
            else
            {
                Debug.LogError("ERROR");
            }
        }

        dic_asciiPairs = asciiPairs;
        hasMixed = true;

    }
    public async Task SetInitialPosWithY(Task signal)
    {
        var tcs = new TaskCompletionSource<bool>();
        await signal;
        Task childTask_1 = new Task(() =>
        {
            WordTableGenerator.EnableGridLayout(false);

            for (int i = 0; i < activeCells.Count; i++)
            {
                activeCells[i].SetAnchroredPosWithY(100f);
            }
            tcs.SetResult(true);
            //Debug.Log(System.Threading.Thread.CurrentThread.ManagedThreadId);
        });

        childTask_1.Start(TaskScheduler.FromCurrentSynchronizationContext());
        await childTask_1;
        await Task.Delay(400);

        Task childTask_2 = new Task(async () =>
        {
            await Task.Yield();
            var lsPrev = activeCells.Reverse<WordTableCell>().ToList();

            Sequence WordCellSeq = DOTween.Sequence();
            //Debug.Log(System.Threading.Thread.CurrentThread.ManagedThreadId);

            for (int i = 0; i < lsPrev.Count; i++)
            {
                WordCellSeq.AppendInterval(.1f);
                Vector2 pos = lsPrev[i].rectTransform.anchoredPosition;
                pos.y -= 100f;

                WordCellSeq.Join(lsPrev[i].rectTransform.DOAnchorPos(pos, .1f));
                WordCellSeq.Join(lsPrev[i].img.DOFade(1f, .1f).From(0f));
                WordCellSeq.Join(lsPrev[i].txt.DOFade(1f, .1f).From(0f));

            }

            await Task.Delay(1000);
            InputManager.I.IsTouchActiveted = true;
        });

        childTask_2.Start(TaskScheduler.FromCurrentSynchronizationContext());
        await childTask_2;

        //WordCellSeq.AppendInterval(1f);
        //WordTableGenerator.EnableGridLayout(true);
    }
    public void SetRows()
    {

        for (int i = 0; i < rInfo.RowLetterCount; i++)
        {
            RowController row = new RowController();
            row.CorrectWord = rInfo.CorrectWords[i];
            row.ArrChar = row.CorrectWord.ToCharArray();


            for (int a = 0; a < rInfo.Row[i].rowLetters.Length; a++)
            {

                row.AddCells(cells[rInfo.Row[i].rowLetters[a].i, rInfo.Row[i].rowLetters[a].j]);
                cells[rInfo.Row[i].rowLetters[a].i, rInfo.Row[i].rowLetters[a].j].myRow = row;
            }

            WordTableGenerator.lsRows.Clear();
            
            WordTableGenerator.lsRows.Add(row);
           
            Debug.Log(WordTableGenerator.lsRows.Count);

        }
    }


}
