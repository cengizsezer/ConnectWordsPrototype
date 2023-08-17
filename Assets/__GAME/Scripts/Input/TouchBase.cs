using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class TouchBase
{
    [HideInInspector]
    internal Vector3 fp, lp;

    public abstract void OnDown();
    public abstract void OnUp();
    public abstract void OnDrag();
    public abstract void OnInitialized();
    public abstract void OnDeinitialized();
}
