using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using MyProject.Core.Manager;

public  class HintPopUp : IPopUp
{
    public int Cost = 100;
    public void Send()
    {
        if(!GameManager.I.isRunning)
        {
            return;
        }

        if (!InputManager.I.IsTouchActiveted) return;


        if(SaveLoadManager.GetCoin()>=Cost)
        {

            int idx = GameInManager.I.GameInController.ConnectionController.SearchWrongCondationIndex();
            Lines _lines;

            if (idx != -1)
            {
                _lines = GameInManager.I.GameInController.ConnectionController.lsConnectNumberCells[idx].mLine;

                GameInManager.I.GameInController.ConnectionController.ResetLine(_lines);
                _lines.DrawHintLine();

                return;
            }

            var bc = GameInManager.I.GameInController.ConnectionController.lsConnectNumberCells.Where(n => n.mLine.connectedCell == null).OrderBy(n => n.VALUE).FirstOrDefault();

            var Index = GameInManager.I.GameInController.ConnectionController.lsLines.IndexOf(bc.mLine);

            _lines = GameInManager.I.GameInController.ConnectionController.lsConnectNumberCells[Index].mLine;


            _lines.DrawHintLine();

            SaveLoadManager.RemoveCoin(Cost);
        }
       


    }


}
