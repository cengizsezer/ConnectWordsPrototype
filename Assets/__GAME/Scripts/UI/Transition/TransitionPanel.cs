using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class TransitionPanel : PersistentSingleton<TransitionPanel>
{
    [Header("Transition")]
    [SerializeField] CanvasGroup cg_transition;
    [SerializeField] Image imgTransition;
    [SerializeField] Canvas canvas;
    public Tween activeTween;
    [SerializeField] List<string> lsHexColor = new();
    private void Start()
    {
        canvas.worldCamera = Camera.main;
    }
    private void Update()
    {
        if(canvas.worldCamera == null)
            canvas.worldCamera = Camera.main;
    }

    Color ToColorFromHex(string hexademical)
    {
        string s = "#" + hexademical;
        Color newCol = Color.white;
        if (ColorUtility.TryParseHtmlString(s, out newCol))
        {
            return newCol;
        }

        return newCol;
    }
    public void FadeTransitionImage(bool show, System.Action onCompleted = null)
    {
        Ease ease = show ? Ease.OutQuad : Ease.InQuad;

        float initialVal = show ? -.5f : .25f;
        float endVal = show ? .25f : -.5f;
        float time = .8f;
        imgTransition.material.SetFloat("_MaskAmount", initialVal);

        var s = lsHexColor[LevelManager.CurrentChapterID];
        var c = ToColorFromHex(lsHexColor[LevelManager.CurrentChapterID]);
        imgTransition.material.SetColor("_Emission", c);

        cg_transition.DOFade(show ? 1f : 0f, time).SetEase(ease);
        cg_transition.blocksRaycasts = true;

        Tween T = DOTween.To(() => initialVal, x => initialVal = x, endVal, time).SetEase(ease).OnUpdate(() =>
        {
            imgTransition.material.SetFloat("_MaskAmount", initialVal);
        }).OnComplete(() =>
        {
            imgTransition.material.SetFloat("_MaskAmount", endVal);

            if (!show)
            {
                cg_transition.blocksRaycasts = false;
            }

            onCompleted?.Invoke();

        });
        activeTween = T;

        s = null;
        c = default;
    }
   
}
