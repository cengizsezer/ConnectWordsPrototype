using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;
using System.Linq.Expressions;
using System;

public class GiftBox : PoolObject
{
    [SerializeField] Transform m_Top;
    [SerializeField] RectTransform _rectTop, _rectTopTarget;
    [SerializeField] Transform m_Bottom;
    [SerializeField] Image m_light;
    [SerializeField] Transform m_FullImage;
    [SerializeField] RectTransform m_OpenedTopPosition;
    public Transform m_Elements;

    public IEnumerator GiftBoxAnimation(RectTransform icon, CanvasGroup cgGift,
        RectTransform passingPos, CanvasGroup cg_ClaimButtons, System.Func<bool> b, Vector2 targetPos,bool isCurve)
    {
        m_FullImage.gameObject.SetActive(false);
        m_Elements.gameObject.SetActive(true);
        Coroutine c = StartCoroutine(OpenPrizePanel(icon, cgGift, passingPos, cg_ClaimButtons, b, targetPos, isCurve));
        yield return c;

    }

    private Func<bool> signalForClaim;
    IEnumerator OpenPrizePanel(RectTransform icon,CanvasGroup cgGift,RectTransform passingPos,
        CanvasGroup cg, Func<bool>  signal, Vector2 targetPos,bool isCurve)
    {

        signalForClaim = signal;
        yield return new WaitForSeconds(.3f);
        RectTransform imgGiftBottom= m_Elements.GetComponent<RectTransform>();
        imgGiftBottom.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, icon.rect.width);
        imgGiftBottom.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, icon.rect.height);
        imgGiftBottom.transform.position = icon.transform.position;
        yield return new WaitForSeconds(.2f);
        cgGift.DOFade(.8f, .9f);

         imgGiftBottom.transform.DOScale(1.5f, .7f);

        yield return TweenHelper.PassingBy(imgGiftBottom.transform,
            icon.transform.position, passingPos.position,
           targetPos, 1.6f, 0f,null, isCurve).WaitForCompletion();

        Vector2 _targetAnchoredPos = imgGiftBottom.anchoredPosition + Vector2.down * 50f;

        yield return imgGiftBottom.DOAnchorPos(_targetAnchoredPos, .9f).SetEase(Ease.OutBounce).WaitForCompletion();

        if(cg!=null)
        {
            yield return cg.DOFade(1f, .3f).From(0f).OnComplete(() =>
            {
                cg.interactable = true;
                cg.blocksRaycasts = true;
            });

            
            yield return new WaitUntil((()=> signalForClaim.Invoke()));
           
        }

        yield return cg.DOFade(0f, .3f).From(0f).OnComplete(() =>
        {
            cg.interactable = false;
            cg.blocksRaycasts = false;

        }).WaitForCompletion();

        
        yield return new WaitForSeconds(.5f);
        Sequence s = DOTween.Sequence();
        s.Append(TweenHelper.PunchScale(m_Elements,null,0.1f,1f));
        s.Join(TweenHelper.ShakePosition(m_Elements,null,14f,0.5f));
        s.Join(TweenHelper.ShakeRotation(m_Elements,null, 14f, 0.5f));
        s.Append(_rectTop.DOAnchorPos(_rectTopTarget.anchoredPosition, 1f).SetEase(Ease.OutBack));
        s.Join(m_Top.DOLocalRotate(new Vector3(0f, 0f, -35f), .5f));
        s.Join(m_light.DOFade(1f, .4f).From(0f));
        yield return s.WaitForCompletion();

    }

    private void OnDisable()
    {
        DOTween.Kill(transform);
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

    public override void OnCreated()
    {
        OnDeactivate();
    }
}
