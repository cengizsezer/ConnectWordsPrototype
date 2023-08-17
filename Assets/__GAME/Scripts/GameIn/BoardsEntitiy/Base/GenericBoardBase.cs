using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

public abstract class GenericBoardBase<T> : IGenericBase<T> where T : Cell
{
    public T[,] cells { get; set; }
    public Color cellEmptyImageColor { get; set; }
    public Color cellEmptyTextColor { get; set; }
    public int rowCount { get; set; }
    public int lineCount { get; set; }
    public List<ColorPairs> lsColorPairs { get; set; }
    public List<ColorPairs> lsTextColors { get; set; }
    public Dictionary<int, Color> colorPairs { get; set; } = new();
    public BoardGenerator BoardGenerator { get; set ; }
    public WordTableGenerator WordTableGenerator { get ; set ; }

    public virtual async Task FillImagePairs() => await Task.Yield();
    public virtual async Task FillTextPairs() => await Task.Yield();
    public virtual async Task CreateBoardCells() => await Task.Yield();
    public virtual async Task SetCellsAvailable() => await Task.Yield();


    public T GetCellAt(int i, int j)
    {
        return cells[i, j];
    }

    public void SetObjColors(T cell) 
    {
        char mChar = System.Convert.ToChar(cell.VALUE);
        cell.img.color = (cell is BoardCell) ? GetColorOfID(mChar) : GetImgColorOfID(mChar);
        cell.defaultImgColor = cell.img.color;
        cell.txt.color = GetTextColorOfID(mChar);
        cell.defaultTextColor = cell.txt.color;
    }

    public virtual Color GetEmptyImageColor() => cellEmptyImageColor;
    public virtual Color GetEmptyTextColor() => cellEmptyTextColor;
    public virtual int GetLineCount() => lineCount;
    public virtual int GetRowCount() => rowCount;

    public Color ToColorFromHex(string hexademical)
    {
        string s = "#" + hexademical;
        Color newCol = Color.white;
        if (ColorUtility.TryParseHtmlString(s, out newCol))
        {
            return newCol;
        }

        return newCol;
    }

    public Color GetImgColorOfID(char id)
    {
        Color c = Color.black;

        for (int i = 0; i < lsColorPairs.Count; i++)
        {
            if (lsColorPairs[i].id == id)
            {
                c = lsColorPairs[i].color;
                return c;
            }
        }

        return c;
    }

    public Color GetTextColorOfID(char id)
    {
        Color c = Color.black;

        for (int i = 0; i < lsTextColors.Count; i++)
        {
            if (lsTextColors[i].id == id)
            {
                c = lsTextColors[i].color;
                return c;
            }
        }

        return c;
    }

    public Color GetColorOfID(int id)
    {
        Color c = Color.black;

        if (colorPairs.TryGetValue(id, out Color a))
        {
            c = new Color(a.r, a.g, a.b, 1f);
            c = a;

            return c;
        }

        return c;
    }

    public virtual async void Spawn() => await Task.Yield();
    
}
