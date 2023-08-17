using System.Collections;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using MyProject.Core.Manager;

public class ChapterFinishUIManager : MenuManager<ChapterFinishUIManager>
{
    [SerializeField] CanvasGroup TopPanel;
    public void ShowPanel()
    {
        EnablePanel(TopPanel, false, null, false);
        EnablePanel(initialCanvas, true, () => StartCoroutine(SuccessRoutine()), false);
    }

    [Header("PRIZE", order = 1)]

    [Header("Canvas Groups", order = 2)]

    [SerializeField] CanvasGroup panelPrize;
    [SerializeField] CanvasGroup btnNextLevelGroup;
    [SerializeField] CanvasGroup cgLevelProgress;
    [SerializeField] RectTransform ParentBG;
    [Header("Texts", order = 2)]

    [SerializeField] TextMeshProUGUI txtPrizeProgress;
    [SerializeField] TextMeshProUGUI txtLevelProgress;
    [SerializeField] TextMeshProUGUI txtCoin;

    [Header("Images")]

    [SerializeField] Image fillPrize;
    [SerializeField] Image fillLevel;
    [SerializeField] Vector2 initPosGiftPrize, initPosChapterPrize;
    [SerializeField] Ease BGEase;
    [SerializeField] float BGMovingDuration;
    [SerializeField] RectTransform Header;
    [SerializeField] Image currentChapterImage;
    [SerializeField] Image futureChapterImage;

    [SerializeField] CanvasGroup ChapterAnimanitonObject;
    [SerializeField] TextMeshProUGUI ChapterSuccessText;
    [SerializeField] CanvasGroup cg_GiftClaimButton;
    private void Start()
    {
        SetCointText();
    }
    public void SetCointText()
    {
        txtCoin.text = SaveLoadManager.GetCoin().ToString();
    }


    [SerializeField] GiftBox giftBoxPrefab;
    [SerializeField] CanvasGroup BlackScreen;
    [SerializeField] RectTransform passingPos;
    [SerializeField] RectTransform giftIcon;


    private void CallShufflingFlyingText(string txt)
    {

        Vector2 flyingTextStartPos = UIEffectsManager.I.LeftOutSidePos.position;
        Vector2 flyingTextWaitingPos = UIEffectsManager.I.CenterPos.position;
        Vector2 flyingTextEndPos = UIEffectsManager.I.RightOutSidePos.position;
        UIEffectsManager.I.CreatePassingByFlyingText(txt, 130, flyingTextStartPos, flyingTextWaitingPos, flyingTextEndPos,initialCanvas.GetComponent<RectTransform>(), 3f, 0);
    }
    IEnumerator SuccessRoutine()
    {
        currentChapterImage.sprite = ChapterImageLoader.GetBackgroundSprite(LevelManager.GetLevelInfo().chapters[LevelManager.CurrentChapterID-1].ChapterName);
        futureChapterImage.sprite= ChapterImageLoader.GetBackgroundSprite(LevelManager.GetLevelInfo().chapters[LevelManager.CurrentChapterID].ChapterName);

        btnNextLevelGroup.interactable = false;
        btnNextLevelGroup.blocksRaycasts = false;
       
        Header.transform.localScale = new Vector3(1f, 1f, 1f);

        int maxChapterID = SaveLoadManager.GetMaxTotalChapter();
        int currentChapterID = LevelManager.CurrentChapterID;
        
        if ( currentChapterID <= maxChapterID)
        {
           
            GameManager.I.currentSprite = ChapterImageLoader.GetBackgroundSprite(LevelManager.GetLevelInfo().chapters[LevelManager.CurrentChapterID].ChapterName);
            GameManager.I.staticBG.sprite = GameManager.I.currentSprite;
            yield return new WaitForSeconds(.4f);
           
            btnNextLevelGroup.DOFade(1f, .3f).OnComplete(() =>
            {
                btnNextLevelGroup.interactable = true;
                btnNextLevelGroup.blocksRaycasts = true;
            });

            yield break;
        }


        SaveLoadManager.IncreaseTotalChapter();
        GiftBox currentGiftBox = null;


        if (PrizeManager.I.IsPrizeAvailable)
        {
            float progressPrize = PrizeManager.I.GetPrizeProgress();
            txtPrizeProgress.SetText(PrizeManager.I.CurrentStep.ToString() + "/" + PrizeManager.I.TotalStep);
            yield return panelPrize.transform.DOScale(1f, .8f).From(0f).OnComplete(() =>
            {
                fillPrize.DOFillAmount(progressPrize, .35f);
            }).WaitForCompletion();

            if (PrizeManager.I.CurrentStep == PrizeManager.I.TotalStep)
            {
                GiftBox _giftBox = Instantiate(giftBoxPrefab, transform);
                currentGiftBox = _giftBox;
                _giftBox.transform.position = panelPrize.transform.position;
                _giftBox.gameObject.SetActive(true);
                yield return _giftBox.GiftBoxAnimation(giftIcon, BlackScreen, passingPos,cg_GiftClaimButton, () => GameInUIManager.I.IsClaim,Vector2.zero,true);
                yield return GameInManager.I.GameInController.CurrencyController.GetPrize(10, 10f, _giftBox.m_Elements.GetComponent<RectTransform>());
                yield return new WaitForSeconds(2f);

            }
        }

        yield return new WaitForSeconds(.7f);

        if (currentGiftBox != null)
        {
            TweenHelper.ShrinkDisappear(currentGiftBox.transform, () => currentGiftBox.gameObject.SetActive(false)).WaitForCompletion();

        }


        txtLevelProgress.SetText(LevelManager.ChapterLength + "/" + LevelManager.ChapterLength);

        yield return cgLevelProgress.transform.DOScale(1f, .8f).From(0f).OnComplete(() =>
        {
            fillLevel.DOFillAmount(1f, .35f);
        }).WaitForCompletion();


        yield return new WaitForSeconds(1f);

        bool resault = (LevelManager.CurrentLevelID <= 4 && LevelManager.CurrentChapterID == 0);

        int count = resault ? 5 : 10;
        float addCoinValue = resault ? 5f : 10f;

        yield return GameInManager.I.GameInController.CurrencyController.GetPrize(count, addCoinValue);

        yield return new WaitForSeconds(1f + (count / 3.5f));

        yield return ChapterAnimanitonObject.transform.DOScale(25f, 2f).From(1f).OnComplete(() =>
        {
            //CallShufflingFlyingText();
            ChapterSuccessText.DOFade(1f, 1f).From(0f);
            GameManager.I.currentSprite = ChapterImageLoader.GetBackgroundSprite(LevelManager.GetLevelInfo().chapters[LevelManager.CurrentChapterID].ChapterName);
            GameManager.I.staticBG.sprite = GameManager.I.currentSprite;

            currentChapterImage.DOFade(0f, 2f).OnComplete(() =>
            {
                ChapterSuccessText.DOFade(0f, 0.7f).From(0f);
                ChapterAnimanitonObject.transform.DOScale(1f, 2f).From(20f);
            }).SetDelay(0.4f);


        }).WaitForCompletion();

        if (LevelManager.IsShowInterstitial())
        {
            
        }

        yield return new WaitForSeconds(4f);
        yield return btnNextLevelGroup.DOFade(1f, .3f).OnComplete(() =>
        {
            btnNextLevelGroup.interactable = true;
            btnNextLevelGroup.blocksRaycasts = true;
        }).WaitForCompletion();

    }
}
