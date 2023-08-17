using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PopUpProcessor
{
    IPopUp PopUp;
    public PopUpProcessor(IPopUp _PopUp)
    {
        PopUp = _PopUp;
    }

    public void Process()
    {
        PopUp.Send();
    }
}
