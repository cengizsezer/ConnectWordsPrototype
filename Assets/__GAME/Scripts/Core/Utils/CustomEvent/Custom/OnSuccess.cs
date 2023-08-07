using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OnSuccess : CustomEvent
{
    public static OnSuccess Create()
    {
        var evnt = new OnSuccess();
        return evnt;
    }
}
