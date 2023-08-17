using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DelayAction
{
    private Action action;
    private float delay;

    public DelayAction(Action action, float delay)
    {
        this.action = action;
        this.delay = delay;
    }

    public void Execute(MonoBehaviour parent)
    {
        parent.StartCoroutine(GetCoroutine());
    }

    private IEnumerator GetCoroutine()
    {
        yield return new WaitForSeconds(delay);
        action?.Invoke();
    }
   
}
