using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class PopUpHandler : Singleton<PopUpHandler>
{
    public PopUpType popUpType = PopUpType.None;
    Dictionary<PopUpType, IPopUp> dcPopUp = new Dictionary<PopUpType, IPopUp>();

    private void Start()
    {
        dcPopUp.Add(PopUpType.Hint, new HintPopUp());

    }
    public IPopUp GetPopUp()
    {
        if(popUpType==PopUpType.Hint)
        {
            return new HintPopUp();
        }

        return null;
    }

    public void AddPopUpType(PopUpType type)
    {
        if (!dcPopUp.ContainsKey(type))
            dcPopUp.Add(PopUpType.Hint, new HintPopUp());

    }


    public IPopUp GetPp(PopUpType type)
    {
        IPopUp value = null;
        if (dcPopUp.ContainsKey(type))
        {
            if (dcPopUp.TryGetValue(type, out IPopUp p))
            {
                value = p;
                return value;
            }

        }
        return value;

    }


}
