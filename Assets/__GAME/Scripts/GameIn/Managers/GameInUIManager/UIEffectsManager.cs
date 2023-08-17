using DG.Tweening;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using MyProject.Extentions;

public sealed class UIEffectsManager : Singleton<UIEffectsManager>
{
    public enum CanvasLayer
    {
        OverGridUnderUI=0,
        OverEverything=1,
        OverNextLevelCanvas=2,
        OverNexChapterCanvas=3
    }
   
    [SerializeField] private RectTransform onGridUnderUiLayerParent;
    [SerializeField] private RectTransform overEverythingLayerParent;
    [SerializeField] private RectTransform overNextLevelParent;
    [SerializeField] private RectTransform overNextChapterParent;
    [SerializeField] private GameObject flyingSpritePrefab;
    [SerializeField] private GameObject flyingTextPrefab;
    [SerializeField] private List<RectTransform> pointReferenceRects = new List<RectTransform>();

    [SerializeField] public RectTransform TopOutSidePos;
    [SerializeField] public RectTransform LeftOutSidePos;
    [SerializeField] public RectTransform RightOutSidePos;
    [SerializeField] public RectTransform CenterPos;

    public void CreateCurvyFlyingSprite(BoardCell cell,Sprite sprite,Color c ,string text, Vector2 spriteSize, Vector2 spawnPos, Vector2 targetPos, float duration,CanvasLayer layer, Action onComplete = null)
    {
        FlyingImage flyingObj = PoolHandler.I.GetObject<FlyingImage>();
        flyingObj.m_ConnectedCell = cell;
        flyingObj.transform.position = spawnPos;
        flyingObj.transform.SetParent(GetLayerParent(layer));
        flyingObj.GetComponent<RectTransform>().sizeDelta = spriteSize * 2f;
        flyingObj.GetComponent<RectTransform>().localScale = new Vector3(1, 1, 1);
        //flyingObj.GetComponent<Image>().sprite = sprite;
        //flyingObj.GetComponent<Image>().color = c;
        flyingObj.GetComponent<Image>().color.SetAlpha(0f);
        TMPro.TMP_Text textComponent = flyingObj.GetComponentInChildren<TMPro.TMP_Text>();
        textComponent.text = text;
        textComponent.rectTransform.sizeDelta = spriteSize*2f;
        textComponent.rectTransform.localScale = new Vector3(1, 1, 1);
        Tween flyingTween = TweenHelper.CurvingMoveTo(flyingObj.transform, targetPos, onComplete, duration, .2f, Ease.InOutCubic, Ease.InBack);
        flyingTween.onComplete += () => flyingObj.OnDeactivate();
    }

    public void CreateLinearFlyingSprite(Sprite sprite, Vector2 spriteSize, Vector2 spawnPos, Vector2 targetPos, CanvasLayer layer, Action onComplete = null)
    {
        CoinBehaviour flyingObj = PoolHandler.I.GetObject<CoinBehaviour>();
       
        flyingObj.transform.position = spawnPos;
        flyingObj.transform.SetParent(GetLayerParent(layer));
        flyingObj.GetComponent<RectTransform>().sizeDelta = spriteSize;
        flyingObj.GetComponent<Image>().sprite = sprite;
        Tween flyingTween = TweenHelper.LinearMoveTo(flyingObj.transform, targetPos, onComplete);
        flyingTween.onComplete += () => flyingObj.OnDeactivate();
    }

    public void CreatePassingByFlyingText(string text, float fontSize, Vector2 spawnPos, Vector2 waitingPos, Vector2 targetPos, RectTransform recTS, float moveDuration, float waitDuration, Action onComplete = null)
    {
        FlyingText flyingObj = PoolHandler.I.GetObject<FlyingText>();
        flyingObj.transform.SetParent(recTS);
        flyingObj.transform.position = spawnPos;
        flyingObj.GetComponent<RectTransform>().localScale = new Vector3(1, 1, 1);
        TMPro.TMP_Text textComponent = flyingObj.GetComponent<TMPro.TMP_Text>();
        textComponent.text = text;
        textComponent.fontSize = fontSize;
        Tween flyingTween = TweenHelper.PassingBy(flyingObj.transform, spawnPos, waitingPos, targetPos, moveDuration, waitDuration, onComplete);
        flyingTween.onComplete += () => flyingObj.OnDeactivate();
    }

    public RectTransform GetLayerParent(CanvasLayer layer)
    {
        switch (layer)
        {
            case CanvasLayer.OverGridUnderUI:
                return onGridUnderUiLayerParent;
            case CanvasLayer.OverEverything:
                return overEverythingLayerParent;
            case CanvasLayer.OverNextLevelCanvas:
                return overNextLevelParent;
            case CanvasLayer.OverNexChapterCanvas:
                return overNextChapterParent;
        }
        return null;
    }


    public CanvasLayer GetTypeLayer(bool on)
    {
        if (!on)
        {
            return CanvasLayer.OverNextLevelCanvas;
        }

        return CanvasLayer.OverNexChapterCanvas;
    }

    public Vector2 GetReferencePointByName(string referenceName)
    {
        foreach (RectTransform rect in pointReferenceRects)
        {
            if (rect.name != referenceName) continue;
            return rect.position;
        }
        Debug.LogError("Reference point with Name not found: " + referenceName);
        return Vector2.zero;
    }

}
