using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TweenHelper : MonoBehaviour
{
    public static Tween BouncyMoveTo(Transform transform, Vector2 targetPos, Action onComplete = null, float duration = .4f)
    {
        Tween tween = transform.DOMove(targetPos, duration).SetEase(Ease.OutBounce);
        if (onComplete != null) tween.onComplete += () => onComplete();
        return tween;
    }

    public static Tween LinearMoveTo(Transform transform, Vector2 targetPos, Action onComplete = null, float duration = .5f)
    {
        Tween tween = transform.DOMove(targetPos, duration).SetEase(Ease.Linear);
        if (onComplete != null) tween.onComplete += () => onComplete();
        return tween;
    }

    public static Tween LinearLocalMoveTo(Transform transform, Vector2 targetPos, Action onComplete = null, float duration = .5f)
    {
        Tween tween = transform.DOLocalMove(targetPos, duration).SetEase(Ease.Linear);
        if (onComplete != null) tween.onComplete += () => onComplete();
        return tween;
    }

    public static Tween Jump(Transform transform, Action onComplete = null, float jumpPower = 30, float duration = .25f)
    {
        Tween tween = transform.DOJump(transform.position, jumpPower, 1, duration).SetEase(Ease.Linear);
        if (onComplete != null) tween.onComplete += () => onComplete();
        return tween;
    }

    public static Tween CurvingMoveTo(Transform transform, Vector2 targetPoint, Action onComplete = null, float duration = .5f,
        float curveAmountMultiplier = .2f, Ease sidewaysEase = Ease.InOutCubic, Ease forwardEase = Ease.InOutCubic)
    {
        float distanceToTarget = Vector2.Distance((Vector2)transform.position, targetPoint);
        Vector2 moveDir = (targetPoint - (Vector2)transform.position).normalized;
        Vector2 moveDirNormal = new Vector2(-moveDir.y, moveDir.x);
        int curveTowards = transform.position.x - targetPoint.x > 0 ? 1 : -1;
        Sequence sequence = DOTween.Sequence();
        sequence.Join(transform.DOBlendableMoveBy(curveTowards * moveDirNormal * distanceToTarget * curveAmountMultiplier, duration / 2).SetEase(sidewaysEase));
        sequence.Append(transform.DOBlendableMoveBy(-curveTowards * moveDirNormal * distanceToTarget * curveAmountMultiplier, duration / 2).SetEase(sidewaysEase));
        sequence.Insert(0, transform.DOBlendableMoveBy(targetPoint - (Vector2)transform.position, duration).SetEase(forwardEase));
        if (onComplete != null) sequence.onComplete += () => onComplete();
        return sequence;
    }

    public static Tween PassingBy(Transform transform, Vector2 spawnPoint, Vector2 waitingPoint, Vector2 targetPoint, float moveDuration, float waitDuration, Action onComplete = null, bool IsCurve = true)
    {
        float distanceToTarget = Vector2.Distance(spawnPoint, waitingPoint);
        Vector2 moveDir = (targetPoint - (Vector2)transform.position).normalized;
        Vector2 moveDirNormal = new Vector2(-moveDir.y, moveDir.x);
        Sequence sequence = DOTween.Sequence();
        sequence.Join(transform.DOBlendableMoveBy(moveDirNormal * distanceToTarget * .2f, moveDuration / 4).SetEase(Ease.InOutQuad));
        sequence.Append(transform.DOBlendableMoveBy(-moveDirNormal * distanceToTarget * .2f, moveDuration / 4).SetEase(Ease.InOutQuad));
        sequence.Insert(0, transform.DOBlendableMoveBy(waitingPoint - spawnPoint, moveDuration / 2).SetEase(Ease.InOutQuad));
        sequence.AppendInterval(waitDuration);

        if(IsCurve)
        {
            distanceToTarget = Vector2.Distance(waitingPoint, targetPoint);
            sequence.Append(transform.DOBlendableMoveBy(-moveDirNormal * distanceToTarget * .2f, moveDuration / 4).SetEase(Ease.InOutQuad));
            sequence.Append(transform.DOBlendableMoveBy(moveDirNormal * distanceToTarget * .2f, moveDuration / 4).SetEase(Ease.InOutQuad));
            sequence.Insert(waitDuration + (moveDuration / 2), transform.DOBlendableMoveBy(targetPoint - waitingPoint, moveDuration / 2).SetEase(Ease.InOutQuad));
        }
       
        if (onComplete != null) sequence.onComplete += () => onComplete();
        return sequence;
    }

    public static Tween ShakeRotation(Transform target, Action onComplete = null, float shakeAngle = 14, float duration = 0.1f)
    {
        int randomDirection = UnityEngine.Random.value < .5 ? 1 : -1;
        Sequence sequence = DOTween.Sequence();
        sequence.Append(target.DOPunchRotation(new Vector3(0, 0, randomDirection * shakeAngle), duration / 2));
        sequence.Append(target.DOPunchRotation(new Vector3(0, 0, -randomDirection * (shakeAngle / 2f)), duration / 2));
        if (onComplete != null) sequence.onComplete += () => onComplete();
        return sequence;
    }

    public static Tween ShakePosition(Transform target, Action onComplete = null, float shakeAngle = 14, float duration = 0.1f)
    {
        int randomDirection = UnityEngine.Random.value < .5 ? 1 : -1;
        Sequence sequence = DOTween.Sequence();
        sequence.Append(target.DOPunchPosition(new Vector3(randomDirection * shakeAngle, 0,0), duration / 2));
        sequence.Append(target.DOPunchPosition(new Vector3(-randomDirection * (shakeAngle / 2f), 0, 0), duration / 2));
        if (onComplete != null) sequence.onComplete += () => onComplete();
        return sequence;
    }

    public static Tween PunchScale(Transform target, Action onComplete = null, float scaleMultiplier = .3f, float duration = .2f)
    {
        Tween tween = target.DOPunchScale(target.localScale * scaleMultiplier, duration);
        if (onComplete != null) tween.onComplete += () => onComplete();
        return tween;
    }

    public static Tween ShrinkDisappear(Transform target, Action onComplete, float duration = .3f)
    {
        Tween tween = target.DOScale(Vector3.zero, duration).SetEase(Ease.InBack);
        if (onComplete != null) tween.onComplete += () => onComplete();
        return tween;
    }

    public static Tween Spin(Transform target, Action onComplete, float duration = .3f, bool infinite = false)
    {
        Sequence sequence = DOTween.Sequence();
        sequence.Join(target.DOBlendableRotateBy(new Vector3(720f, 0f, 0f), duration, RotateMode.FastBeyond360).SetEase(Ease.InOutCubic));
        sequence.Insert(0, target.DOBlendableRotateBy(new Vector3(720f, 0f, 0f), duration, RotateMode.FastBeyond360).SetEase(Ease.Linear));
        if (infinite) sequence.SetLoops(-1);
        if (onComplete != null) sequence.onComplete += () => onComplete();
        return sequence;
    }

    public static Tween ButtonClickAnimation(Transform target, Action onComplete=null)
    {
        DOTween.Kill(target);
        float animationTime = .2f;
        Tween tween= target.DOScale(1.2f, animationTime).SetEase(Ease.OutBack).SetId(target).OnComplete(() =>
        {
            target.DOScale(1, animationTime).SetEase(Ease.OutBack).SetId(target).OnComplete(()=> onComplete?.Invoke());
        });
       
       
        return tween;

    }
}
