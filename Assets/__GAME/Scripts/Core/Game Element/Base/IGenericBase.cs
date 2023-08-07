using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;




public interface IGenericBase<T> where T: Cell
{
    BoardGenerator BoardGenerator { get; set; }
    WordTableGenerator WordTableGenerator { get; set; }
    T[,] cells { get; set; }

    Dictionary<int, Color> colorPairs { get; set; }
    Color cellEmptyImageColor { get; set; }
    Color cellEmptyTextColor { get; set; }
    int rowCount { get; set; }
    int lineCount { get; set; }

    List<ColorPairs> lsColorPairs { get; set; }
    List<ColorPairs> lsTextColors { get; set; }
    Color GetEmptyImageColor();
    Color GetEmptyTextColor();
    int GetRowCount();
    int GetLineCount();

    T GetCellAt(int i, int j);
    void SetObjColors(T wtc);
    Task FillTextPairs();
    Task FillImagePairs();

    Task CreateBoardCells();

    Task SetCellsAvailable();
    Color ToColorFromHex(string hexademical);
    Color GetColorOfID(int id);

    Color GetTextColorOfID(char id);

    Color GetImgColorOfID(char id);

    void Spawn();
}

[System.Serializable]
public struct ColorPairs
{
    public char id;
    public Color color;
}

