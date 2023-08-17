using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Lines
{
    public List<BoardCell> lsCells;
    public List<BoardCell> lsHintLinesCells;
    public BoardCell initCell,connectedCell;
    public Color LineColor;
   
    public void SetLastCell(BoardCell bc, bool includeThis)
    {
        List<BoardCell> lsTobeRemoved = new List<BoardCell>();
        bool hasFound = false;

        foreach(BoardCell _bc in lsCells)
        {
            if (!hasFound)
            {
                if (_bc == bc)
                {
                    hasFound = true;
                    if (includeThis)
                    {
                        lsTobeRemoved.Add(_bc);
                        Debug.Log(_bc);
                    }
                   

                }
            }
            else
            {
                lsTobeRemoved.Add(_bc);
            }
            
        }

        foreach (BoardCell _bc in lsTobeRemoved)
        {
            lsCells.Remove(_bc);
            _bc.mLine = null;
        }

        DrawWithRemove(lsTobeRemoved);
        Draw(true);
    }

    public BoardCell GetLastHintCell()
    {
        return lsHintLinesCells.LastOrDefault();
    }
    public void DrawHintLine()
    {

        for (int i = 1; i < lsHintLinesCells.Count; i++)
        {
            if (lsHintLinesCells[i].HasLineOnMe())
            {
                BoardCell Bc = lsHintLinesCells[i];
               
                Debug.Log(Bc.name);
                Bc.mLine.initCell.ResetWordCellText(); 
                Bc.mLine.SetLastCell(Bc, true);
                Bc.mLine = this;
                Bc.mLine.AddCell(Bc);
                Bc.IsHintCell = true;
                Bc.mLine.initCell.IsHintCell = true;
                

            }
            else
            {
                
                AddCell(lsHintLinesCells[i]);
                lsHintLinesCells[i].IsHintCell = true;
                lsHintLinesCells[i].mLine.initCell.IsHintCell = true;
            }
        }
      
        BoardCell WordBC = GetLastHintCell();
        UIPointer.I.Connection(WordBC,lsHintLinesCells[0].mLine);

    }

    public void AddCell(BoardCell bc)
    {
        if (!lsCells.Contains(bc))
            lsCells.Add(bc);

        bc.mLine = this;
        Draw(false);
    }

    public Lines(BoardCell _initCell)
    {
        lsCells = new List<BoardCell>();
        lsHintLinesCells = new List<BoardCell>();
        this.initCell = _initCell;
        lsCells.Add(initCell);
    }

    public void DrawWithRemove(List<BoardCell> tbr)
    {
        BoardGenerator boardGenerator = GameInManager.I.GameInController.GeneratorController.BoardGenerator;
        foreach (BoardCell cell in tbr)
        {
            
            cell.lineImg.enabled = false;
            cell.img.color = boardGenerator.GetBoard().GetEmptyImageColor();
            cell.txt.color = boardGenerator.GetBoard().GetEmptyTextColor();
            
        }

    }

    public void Draw(bool reCreate)
    {
        if(lsCells.Count == 1)//tek secili cell kaldıysa line reset.
        {
            ResetLine(lsCells);
           
        }
        else
        {

            if (reCreate)// cross veya remove isleminde yeniden listeye göre line olusturuyoruz.
            {
                DeActiveLine(lsCells);
                

                for (int i = 0; i < lsCells.Count -1; i++)
                {
                    MovingLineImage(lsCells[i], lsCells[i + 1]);
                    CalculateLineImageScale(lsCells[i], lsCells[i + 1]);
                }

                SetLineColor(lsCells);
               
            }
            else // yeni bir line çiziliyorsa
            {

                for (int i = 0; i < lsCells.Count - 1; i++)
                {
                    MovingLineImage(lsCells[i], lsCells[i + 1]);
                    CalculateLineImageScale(lsCells[i], lsCells[i + 1]);
                }

            }

            SetLineColor(lsCells);
            ReorderChildren(lsCells);
           
        }
    }


    public void ResetLine(List<BoardCell> ls)
    {
        foreach (BoardCell cell in ls)
        {
            cell.lineImg.enabled = false;
            cell.txt.color = cell.defaultTextColor;
        }
    }
    public void SetLineColor(List<BoardCell> ls)
    {
        for (int i = 1; i < ls.Count; i++)
        {
            ls[i].txt.color = Color.white;
            ls[i].img.color = initCell.img.color;
        }
    }

    public void DeActiveLine(List<BoardCell> ls)
    {
        foreach (BoardCell cell in ls)
        {
            cell.lineImg.enabled = false;
        }
    }


    public void ReorderChildren(List<BoardCell> ls)
    {
        for (int i = 0; i < ls.Count; i++)
        {
            lsCells[lsCells.Count - 1 - i].transform.SetSiblingIndex(i);
        }
    }


    public void MovingLineImage(BoardCell thisCell, BoardCell prevCell)
    {
        thisCell.lineImg.enabled = true;
        thisCell.txt.color = Color.white;
        thisCell.lineImg.color = initCell.img.color;
        thisCell.lineImg.transform.position = (thisCell.transform.position + prevCell.transform.position) / 2f;
    }

    //rt.offsetMin = new Vector2(left, rt.offsetMin.y); ------->setleft
    //rt.offsetMax = new Vector2(-right, rt.offsetMax.y);------->setright
    //rt.offsetMax = new Vector2(rt.offsetMax.x, -top);------->settop
    //rt.offsetMin = new Vector2(rt.offsetMin.x, bottom);-------setbottom
    public void CalculateLineImageScale(BoardCell thisCell,BoardCell prevCell)
    {
        if (thisCell.i - prevCell.i < 0 || (thisCell.i - prevCell.i) > 0)
        {
            thisCell.lineImg.rectTransform.localScale = new Vector3(1f, .5f, 1f);
            thisCell.lineImg.rectTransform.offsetMin = new Vector2(0f, thisCell.lineImg.rectTransform.offsetMin.y);
            thisCell.lineImg.rectTransform.offsetMax = new Vector2(0f, thisCell.lineImg.rectTransform.offsetMax.y);
            thisCell.lineImg.sprite = null;
            prevCell.lineImg.sprite = null;

        } else if ((thisCell.j - prevCell.j) < 0 || (thisCell.j - prevCell.j) > 0)
        {
           
            thisCell.lineImg.rectTransform.localScale = new Vector3(.3f, 1f, 1f);
            thisCell.lineImg.rectTransform.offsetMin = new Vector2(thisCell.lineImg.rectTransform.offsetMin.x, 0f);
            thisCell.lineImg.rectTransform.offsetMax = new Vector2(thisCell.lineImg.rectTransform.offsetMax.x, 0f);
            thisCell.lineImg.sprite = GameInManager.I.GameInController.ConnectionController.RightLeftSprite;

        }

    }

   
}


