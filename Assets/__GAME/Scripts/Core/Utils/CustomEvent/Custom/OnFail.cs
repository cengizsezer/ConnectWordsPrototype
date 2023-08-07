using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OnFail : CustomEvent
{
    public static OnFail Create()
    {
        var evnt = new OnFail();
        return evnt;
    }
}
