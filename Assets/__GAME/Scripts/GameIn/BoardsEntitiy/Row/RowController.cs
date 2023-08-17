using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Threading.Tasks;
using UnityEngine;
using System.Linq;

[System.Serializable]
public class RowController
{
    public List<WordTableCell> lsRowCells;
    

    public string CorrectWord;
    public char[] ArrChar;
   

    public void AddCells(WordTableCell cell)
    {
        lsRowCells.Add(cell);
    }
   
    public bool FillControl()
    {
        return lsRowCells.TrueForAll(n=>n.IsFilled);
    }

    public bool IsCorrect()
    {
        ArrChar = CorrectWord.ToCharArray();
       
        for (int i = 0; i < lsRowCells.Count; i++)
        {
            char mChar = Convert.ToChar(lsRowCells[i].VALUE);
           
            if(ArrChar[i]==mChar)
            {
                lsRowCells[i].IsCorrect = true;
            }
            else
            {
                return false;
            }
        }

        bool resault = lsRowCells.TrueForAll(n=>n.IsCorrect);
        return resault;
    }

    public string CharToString(char[] arr)
    {
        System.Text.StringBuilder builder = new System.Text.StringBuilder();

        foreach (char value in arr)
        {
            builder.Append(value);
        }
        return builder.ToString();
    }



    public RowController()
    {
        lsRowCells = new List<WordTableCell>();
       
    }
   
}



