using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using TMPro;
using System.Threading.Tasks;
using System;
using System.Linq;
using MyProject.Constant;

public class LevelSuccessUIManager : MenuManager<LevelSuccessUIManager>
{
    [Header("PRIZE",order = 1)]

    [Header("Canvas Groups",order = 2)]

    [SerializeField] CanvasGroup panelPrize;
    [SerializeField] CanvasGroup btnNextLevelGroup;
    [SerializeField] CanvasGroup cgLevelProgress;
    [SerializeField] RectTransform ParentBG;
    [SerializeField] CanvasGroup TopPanel;
    [Header("Texts", order = 2)]

    [SerializeField] TextMeshProUGUI txtPrizeProgress;
    [SerializeField] TextMeshProUGUI txtLevelProgress;
    [SerializeField] TextMeshProUGUI Btn_LevelText;
    [SerializeField] TextMeshProUGUI txt_Coin;
    [Header("Images")]
    [SerializeField] Ease BGEase;
    [SerializeField] float BGMovingDuration;
    [SerializeField] Image fillPrize;
    [SerializeField] Image fillLevel;

    [SerializeField] RectTransform Header;
    [SerializeField]Image currentChapterImage;
   
    [SerializeField] Vector2 initPosGiftPrize, initPosChapterPrize;
    [SerializeField] GiftBox giftBoxPrefab;
    [SerializeField] CanvasGroup BlackScreen;
    [SerializeField] RectTransform passingPos;
    [SerializeField] RectTransform giftIcon;
    [SerializeField] CanvasGroup cg_GiftClaimButton;
    private void Start()
    {
        SetCointText();
        
    }
    public  void ShowNextLevelPanel()
    {
        EnablePanel(initialCanvas, true, () => StartCoroutine(SuccessRoutine()), false);
        
    }

    public void SetCointText()
    {
        txt_Coin.text = SaveLoadManager.GetCoin().ToString();
    }
    IEnumerator SuccessRoutine()
    {
        
       
        int maxLevelID = SaveLoadManager.GetMaxTotalLevel();
        int currentLevel = SaveLoadManager.GetTotalLevel()+1;

       
        currentChapterImage.sprite = GameManager.I.currentSprite;
        btnNextLevelGroup.interactable = false;
        btnNextLevelGroup.blocksRaycasts = false;
        EnablePanel(TopPanel, false, null, false);
        Header.transform.localScale = new Vector3(1f, 1f, 1f);

        if (maxLevelID >= currentLevel)
        {
            yield return new WaitForSeconds(.4f);
            Btn_LevelText.text = string.Join(" ", "LEVEL", (LevelManager.CurrentLevelID + 1 + (LevelManager.CurrentChapterID * 12)).ToString());
            btnNextLevelGroup.DOFade(1f, .3f).OnComplete(() =>
            {
                btnNextLevelGroup.interactable = true;
                btnNextLevelGroup.blocksRaycasts = true;
            });

            yield break;
        }
        
        GiftBox currentGiftBox = null;
        if (PrizeManager.I.IsPrizeAvailable)
        {
            yield return new WaitForSeconds(.3f);
            float progressPrize = PrizeManager.I.GetPrizeProgress();
            float crntProgressPrize = PrizeManager.I.GetCurrentPrizeProgress();
            
           
            fillPrize.fillAmount = 0f;
            txtPrizeProgress.SetText((PrizeManager.I.CurrentStep - 1f).ToString() + "/" + PrizeManager.I.TotalStep);
            yield return fillPrize.DOFillAmount(crntProgressPrize, 0f).WaitForCompletion();


            yield return panelPrize.transform.DOScale(1f, .8f).From(0f).OnComplete(() =>
            {
                txtPrizeProgress.SetText(PrizeManager.I.CurrentStep.ToString() + "/" + PrizeManager.I.TotalStep);
                fillPrize.DOFillAmount(progressPrize, .35f);
                panelPrize.transform.localScale = new Vector3(1f, 1f, 1f);
            }).WaitForCompletion();

            if (PrizeManager.I.CurrentStep == PrizeManager.I.TotalStep)
            {
                GiftBox _giftBox = Instantiate(giftBoxPrefab, transform);
                currentGiftBox = _giftBox;
                _giftBox.transform.position = panelPrize.transform.position;
                _giftBox.gameObject.SetActive(true);
               
                yield return _giftBox.GiftBoxAnimation(giftIcon, BlackScreen, passingPos, cg_GiftClaimButton, ()=> GameInUIManager.I.IsClaim,Vector2.zero,true);
                yield return GameInManager.I.GameInController.CurrencyController.GetPrize(10, 10f, _giftBox.m_Elements.GetComponent<RectTransform>());
                yield return new WaitForSeconds(2f);
            }
        }


        yield return new WaitForSeconds(.7f);
        if (currentGiftBox != null)
        {
            TweenHelper.ShrinkDisappear(currentGiftBox.transform, () => currentGiftBox.gameObject.SetActive(false)).WaitForCompletion();

        }

        float progressLevel = Mathf.Clamp01((float)LevelManager.CurrentLevelID / (float)LevelManager.ChapterLength);

        bool progresCondition = (LevelManager.CurrentLevelID - 1f == 0);
        float crntProgressLevel = progresCondition ? 0f : Mathf.Clamp01((LevelManager.CurrentLevelID-1f) / LevelManager.ChapterLength);
        fillLevel.fillAmount = 0f;
        txtLevelProgress.SetText((LevelManager.CurrentLevelID-1f) + "/" + LevelManager.ChapterLength);
        yield return fillLevel.DOFillAmount(crntProgressLevel, 0f).WaitForCompletion();

       
       
        yield return cgLevelProgress.transform.DOScale(1f, .8f).From(0f).OnComplete(() =>
        {
            txtLevelProgress.SetText(LevelManager.CurrentLevelID + "/" + LevelManager.ChapterLength);
            fillLevel.DOFillAmount(progressLevel, .35f);

        }).WaitForCompletion();

        yield return new WaitForSeconds(1f);

        bool resault = (LevelManager.CurrentLevelID <= 4 && LevelManager.CurrentChapterID == 0);

        int count = resault ? Constants.CoinSettings.FirstLevelsCount : Constants.CoinSettings.GameCoinCount;
        float addCoinValue = resault ? Constants.CoinSettings.GameCoinValue : Constants.CoinSettings.GameCoinValue;

        yield return GameInManager.I.GameInController.CurrencyController.GetPrize(count, addCoinValue);

        yield return new WaitForSeconds(.5f+(count/3.5f));
        
        if(LevelManager.IsShowInterstitial())
        {
           
        }
        


        Btn_LevelText.text = string.Join(" ", "LEVEL", (LevelManager.CurrentLevelID + 1+(LevelManager.CurrentChapterID*12)).ToString());
        btnNextLevelGroup.DOFade(1f, .3f).OnComplete(() =>
        {
            btnNextLevelGroup.interactable = true;
            btnNextLevelGroup.blocksRaycasts = true;
        });



    }

    
}
