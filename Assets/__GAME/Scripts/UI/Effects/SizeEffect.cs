using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class SizeEffect : EffectBase
{
    [SerializeField] Rules[] rules;

    Tween activeTween;
    Vector3 initialScale;
    Coroutine mRoutine = null;

    public override void Effect()
    {
        base.Effect();

        if (mRoutine != null)
            StopCoroutine(mRoutine);

        if (activeTween != null)
            activeTween.Kill(true);

        initialScale = transform.localScale;

        mRoutine = StartCoroutine(PlayOnceRoutine());

    }

    IEnumerator PlayOnceRoutine()
    {
        for (int i = 0; i < rules.Length; i++)
        {
            yield return transform.DOScale(rules[i].targetSize, rules[i].time).SetEase(rules[i].ease).WaitForCompletion();
        }
    }

    public override void OnEffectStopped()
    {
        base.OnEffectStopped();
        if (activeTween != null)
            activeTween.Kill();

        transform.localScale = initialScale;
    }

    [System.Serializable]
    public class Rules
    {
        public float targetSize, time;
        public Ease ease;
    }
}
