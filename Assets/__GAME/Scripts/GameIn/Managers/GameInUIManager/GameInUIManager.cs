using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using MyProject.Core.Manager;

public class GameInUIManager : MenuManager<GameInUIManager>
{
    //[SerializeField] CanvasGroup panelSettings;
    private Tween _activeTween;
    public Transform target;
    [SerializeField] GameObject[] arrClosedOfTheLevelEnd;
    
    [SerializeField] GameObject Board;
    [SerializeField] ExpandableUISystem expandableUISystem;
    [SerializeField] Image staticBg;
    [SerializeField] Image img_InGameShadow;
    [SerializeField] ExpandableChild[] arrGameInSettingsElement;
    [SerializeField] Sprite[] arrClosedSprites;
    [SerializeField] Sprite[] arrOpenedSprites;
    [SerializeField] TextMeshProUGUI txtCoin;
    public RectTransform PanelBoard;
    public RectTransform Hint;
    public RectTransform btn_Replay;
    private void CompleteLastTween()
    {
        if (_activeTween != null) _activeTween.Complete();
    }
   
   
    public override void OnStart()
    {
        TransitionPanel.I.FadeTransitionImage(false);
        base.OnStart();
        expandableUISystem.Init(expandableUISystem.transform);
        EventManager.RegisterHandler<OnWrongFeedBack>(ShakeFeedBack);
        GameManager.I.staticBG = staticBg;
        staticBg.sprite = GameManager.I.currentSprite;
        SetCointText();
    }

    private void OnDisable()
    {
        EventManager.UnregisterHandler<OnWrongFeedBack>(ShakeFeedBack);
    }
   
    
    public void ShakeFeedBack(OnWrongFeedBack wp)
    {
        Shake(target);
    }
    [SerializeField] WordTableGenerator wordTableGenerator;
    public async Task FadeShadow()
    {
        //Debug.Log(System.Threading.Thread.CurrentThread.ManagedThreadId + ".Thread" +"-------->>>>" + "ShrinkDisappearWordCell");
        await ImageFade(img_InGameShadow);
    }

    public async Task ImageFade(Image img)
    {
        //Debug.Log(System.Threading.Thread.CurrentThread.ManagedThreadId + ".Thread" + "islem" + "----ImageFade");
        img.DOFade(0f, 0.1f).From(1f);
        foreach (var item in arrClosedOfTheLevelEnd)
        {
            item.gameObject.SetActive(false);
        }

        await Task.Yield();
    }


    public void Shake(Transform target, Action onComplete = null)
    {
        CompleteLastTween();
        Sequence tween = DOTween.Sequence();
        tween.Append(TweenHelper.ShakePosition(target, null, 20, 0.15f));
        if (onComplete != null) tween.onComplete = () => { onComplete(); };
        _activeTween = tween;
    }

    public void SetCointText()
    {
        txtCoin.text = SaveLoadManager.GetCoin().ToString();
    }

    public void SetSound()
    {
        SaveLoadManager.SetSound(!SaveLoadManager.IsSoundAvailable());
        int idx = (int)ExpandableChildType.Sound;
        if (!SaveLoadManager.IsSoundAvailable())
        {
           
            arrGameInSettingsElement[idx].img.sprite = arrClosedSprites[idx];
        }
        else
        {
            arrGameInSettingsElement[idx].img.sprite = arrOpenedSprites[idx];
        }
    }
    public void SetMusic()
    {

        SaveLoadManager.SetMusic(!SaveLoadManager.IsMusicAvailable());
        int idx = (int)ExpandableChildType.Music;
        if (!SaveLoadManager.IsMusicAvailable())
        {
            
            arrGameInSettingsElement[idx].img.sprite = arrClosedSprites[idx];
        }
        else
        {
            arrGameInSettingsElement[idx].img.sprite = arrOpenedSprites[idx];
        }


    }
    public void SetHaptic()
    {
        SaveLoadManager.SetHaptic(!SaveLoadManager.IsHapticOn());
        int idx = (int)ExpandableChildType.Haptic;
        if (!SaveLoadManager.IsHapticOn()==false)
        {
           
            arrGameInSettingsElement[idx].img.sprite = arrClosedSprites[idx];
        }
        else
        {
            arrGameInSettingsElement[idx].img.sprite = arrOpenedSprites[idx];
        }

        
    }

    public bool IsClaim = false;

    public void OnClickClaim()
    {
        IsClaim = true;
    }

    public void OnRewardedClaim()
    {
        IsClaim = false;
    }
    public void GoHome()
    {
        TransitionPanel.I.FadeTransitionImage(true, () => StartCoroutine(AsyncLoadMainScene()));
    }
    [SerializeField] CanvasGroup panelShop;
    [SerializeField] RectTransform ShopBGParent;
    [SerializeField] float ChapterBGMovingDuration;
    [SerializeField] float ChapterBGMovingEndValue;
    [SerializeField] Ease ChapterBGMovingEase;
    public void OpenShopPanel(bool isOpen)
    {

        EnablePanel(panelShop, isOpen);
        ShopBGParent.DOAnchorPosX(0f, ChapterBGMovingDuration).SetEase(ChapterBGMovingEase);
    }

    public void CloseButton(bool open)
    {
        EnablePanel(initialCanvas, open);
        ShopBGParent.DOAnchorPosX(-1080f, ChapterBGMovingDuration).SetEase(ChapterBGMovingEase);
    }
    IEnumerator AsyncLoadMainScene()
    {
        var progress = SceneManager.LoadSceneAsync(1, LoadSceneMode.Single);

        while (!progress.isDone)
        {
            yield return null;
        }

        TransitionPanel.I.activeTween.Kill();
        TransitionPanel.I.activeTween = null;
    }

    public void Replay()
    {
        Tween t = TweenHelper.ButtonClickAnimation(btn_Replay, () => {
            SceneManager.LoadScene(2);
            DOTween.KillAll();
        });
    }

    public void OnLevelSuccess()
    {
        

        InputManager.I.IsTouchActiveted = false;

        if (!LevelManager.IsLastLevelOfChapter)
        {
           
            LevelSuccessUIManager.I.ShowNextLevelPanel();
        }
        else
        {

            ChapterFinishUIManager.I.ShowPanel();

            if(LevelManager.AllLevelFinished())
            {
            }

        }
            
    }

    public void LoadNextLevel(Transform target)
    {
        Tween t = TweenHelper.ButtonClickAnimation(target,()=> {
            SceneManager.LoadScene(2);
            DOTween.KillAll();
        });

    }

}
