using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class ShineEffect : EffectBase
{
    [SerializeField] GameObject effectObject;
    [SerializeField] Animator anim;
    [SerializeField] float animationSpeed = 1f;
    private Vector3 InitialPosition;
   
    public bool IsAnimation = true;
    public override void Effect()
    {

        base.Effect();

        InitialPosition = effectObject.transform.position;

        if(IsAnimation)
        {
            anim.speed = animationSpeed;
            anim.SetTrigger("Play");
        }
        else
        {
            MoveEffectObject();
        }
      
    }

    public Tween MoveEffectObject(Action onComplate = null)
    {
        if(anim!=null)
        {
            anim.enabled = false;
        }
       
        CompleteLastTween();
        Vector3 targetPos = effectObject.transform.position + Vector3.right *10f;
        Tween flyingTween = TweenHelper.LinearLocalMoveTo(effectObject.transform, targetPos, onComplate,1f);
        flyingTween.onComplete += () =>
        {
            effectObject.transform.position = InitialPosition;
            CacheTween(flyingTween);
        };


        return flyingTween;
    }

    protected Tween _lastTween = null;

    public void CacheTween(Tween tween)
    {
        _lastTween = tween;
    }

    protected void CompleteLastTween()
    {
        if (_lastTween != null) _lastTween.Complete(true);
    }

    protected void KillLastTween()
    {
        if (_lastTween == null) return;
        _lastTween.Kill(true);
    }

    public override void OnEffectStarted()
    {
        base.OnEffectStarted();
       
        if (effectObject)
            effectObject.SetActive(true);


    }

    public override void OnEffectStopped()
    {
        base.OnEffectStopped();
      
        if (effectObject)
            effectObject.SetActive(false);

    }
}
