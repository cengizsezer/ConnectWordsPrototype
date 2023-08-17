using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public enum PopUpType
{
    Hint = 0,
    None
}
public class PopUpController
{

    public PopUpType popUpType = PopUpType.None;
    Dictionary<PopUpType, IPopUp> dcPopUp = new Dictionary<PopUpType, IPopUp>();


    public PopUpController()
    {
        dcPopUp.Add(PopUpType.Hint, new HintPopUp());
    }
   
    public IPopUp GetPopUp()
    {
        if (popUpType == PopUpType.Hint)
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
