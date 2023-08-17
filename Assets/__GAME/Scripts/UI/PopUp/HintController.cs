using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class HintController
{
    public List<BoardCell> lsHintCells;
    public BoardCell myNumberCell;


    public HintController()
    {
        lsHintCells = new List<BoardCell>();
    }

    public void AddHintCells(BoardCell bc)
    {
        if (!lsHintCells.Contains(bc))
        {
            lsHintCells.Add(bc);
        }
        else
        {
            Debug.Log("bok");
        }
    }
}
