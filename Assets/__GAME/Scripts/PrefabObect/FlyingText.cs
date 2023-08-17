using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlyingText : PoolObject
{
    public override void OnCreated()
    {
        OnDeactivate();
    }

    public override void OnDeactivate()
    {
        PoolHandler.I.EnqueObject(this);
        transform.SetParent(null);
        gameObject.SetActive(false);
    }

    public override void OnSpawn()
    {
        gameObject.SetActive(true);
    }
}
