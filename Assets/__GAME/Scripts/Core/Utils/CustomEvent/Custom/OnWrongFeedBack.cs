using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OnWrongFeedBack : CustomEvent
{
    public static OnWrongFeedBack Create()
    {
        OnWrongFeedBack wFB = new OnWrongFeedBack();
        return wFB;
    }
}
