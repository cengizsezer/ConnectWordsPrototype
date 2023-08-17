using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum AnimationState
{
    Start,
    Flying,
    Complate
}
public class FlyingImage : PoolObject
{
    public BoardCell m_ConnectedCell;
    public AnimationState m_State;
    

    private void Update()
    {
        if(m_State==AnimationState.Flying)
        {
            if(m_ConnectedCell.mLine==null)
            {
                DOTween.Kill(transform);
                OnDeactivate();
            }
        }
    }
    public override void OnCreated()
    {
        OnDeactivate();
    }

    public override void OnDeactivate()
    {
        PoolHandler.I.EnqueObject(this);
        transform.SetParent(null);
        m_State = AnimationState.Complate;
        gameObject.SetActive(false);
    }

    public override void OnSpawn()
    {
        gameObject.SetActive(true);
        m_State = AnimationState.Start;
    }

   
}
