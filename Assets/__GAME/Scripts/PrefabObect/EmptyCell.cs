using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EmptyCell : PoolObject
{
    [SerializeField] Transform spinObject;
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


    public void LevelEndSequence()
    {
        Sequence s = DOTween.Sequence();
        Vector3 targetPos = transform.position;
        //targetPos.y = transform.position.y - -100f;
        s.Append(TweenHelper.LinearLocalMoveTo(transform,targetPos,null,.9f));
        s.Join(TweenHelper.Spin(spinObject.transform,null,1f));
        
    }
   
}
