using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GiftPanelController:PanelController<GiftPanelController>,IDisposable
{
    private CanvasGroup cg_ClaimButtons;
    private CanvasGroup BlackScreen;
    private CanvasGroup panelGift;

    private RectTransform GiftBGParent;
    private RectTransform passingPos;
    private RectTransform giftIcon;

    private RectTransform GiftPanelSafeArea;
    private RectTransform GiftPanelCoinParent;
    private RectTransform target;
    private RectTransform layer;

   
    private Image giftActivetedImage;
    private Sprite CoinSprite;

    private Button giftButton;
    private Button claimButton;
    private Button claimRewardedButton;

    private float giftPanelBGMovingDuration;
    private float giftPanelBGMovingEndValue;
    private Ease giftPanelBGMovingEase;
    int currentGoldCount = 10;
    public bool IsClaim = false;



    public GiftPanelController():this(null,null)
    {

    }
    public GiftPanelController(GiftPanelControllerSettings settings,GiftPanelControllerReferences references)
    {
        this.initialCanvas = references.InitialCanvas;
        activePanel = initialCanvas;

        cg_ClaimButtons = references.CanvasGroupClaimButtons;
        BlackScreen = references.BlackScreenCanvas;
        panelGift = references.GiftPanel;
        GiftBGParent = references.GiftBGParent;
        passingPos = references.PassingPosition;
        giftIcon = references.GiftIcon;
        GiftPanelSafeArea = references.GiftPanelSafeArea;
        GiftPanelCoinParent = references.GiftPanelCoinParent;
        target = references.GiftPanelCoinTarget;
        layer = references.GiftPanelCoinLayer;
        giftActivetedImage = references.GiftActivetedImage;
        CoinSprite = references.CoinSprite;
        giftButton = references.GiftPanelOpenButton;
        claimButton = references.ClaimButton;
        claimRewardedButton= references.ClaimRewardedButton;
        giftActivetedImage.gameObject.SetActive(IsCanGivenGold());


        giftPanelBGMovingDuration = settings.GiftPanelBGMovingDuration;
        giftPanelBGMovingEase = settings.GiftPanelBGMovingEase;

        giftButton.onClick.RemoveAllListeners();
        claimButton.onClick.RemoveAllListeners();
        claimRewardedButton.onClick.RemoveAllListeners();

        giftButton.onClick.AddListener(()=>OpenGiftPanel(true));
        claimButton.onClick.AddListener(()=> OnGiftButton());
        claimRewardedButton.onClick.AddListener(()=> OnGiftRewardad());
    }

    public bool IsCanGivenGold()
    {


        if (DailyPrizeManager.isRewardAvailable)
        {

            if (SaveLoadManager.IsTookTheGold())
            {
                return false;
            }

            return true;
        }
        else
        {
            if (!SaveLoadManager.IsTookTheGold())
            {
                return true;
            }
        }


        return false;
    }
    public void OpenGiftPanel(bool isOpen)
    {

        if (!IsCanGivenGold()) return;

        EnablePanel(panelGift, isOpen);
        GiftBGParent.DOAnchorPosX(0f, giftPanelBGMovingDuration).SetEase(giftPanelBGMovingEase);
        IsClaim = false;
        CoroutineMonoBehaviourParent.Instance.StartCoroutine(GiftBoxStartAnimation(() => currentGoldCount,() => IsClaim));
    }

    private IEnumerator GiftPanelOpenedStartAnimation(Func<bool> resault)
    {
        yield return CoroutineMonoBehaviourParent.Instance.StartCoroutine(GiftBoxStartAnimation(() => currentGoldCount, resault));
    }

    private IEnumerator GiftBoxStartAnimation(Func<int> count, Func<bool> signal)
    {

        GiftBox currentGiftBox;
        GiftBox _giftBox = PoolHandler.I.GetObject<GiftBox>();
        _giftBox.transform.SetParent(panelGift.transform);
        GiftPanelSafeArea.SetAsFirstSibling();
        GiftPanelCoinParent.SetAsLastSibling();
        currentGiftBox = _giftBox;
        _giftBox.transform.position = giftIcon.transform.position;
        _giftBox.transform.localScale = Vector3.one;
        _giftBox.gameObject.SetActive(true);

        yield return _giftBox.GiftBoxAnimation(giftIcon, BlackScreen, passingPos,
            cg_ClaimButtons, signal, Vector2.one * 2f, false);

        Task t = GetPrize(count, layer, target, _giftBox.m_Elements.GetComponent<RectTransform>());
        yield return new WaitUntil(() => t.IsCompleted);

        t = null;

        if (currentGiftBox != null)
        {
            yield return TweenHelper.ShrinkDisappear(currentGiftBox.transform, () =>
            {
                currentGiftBox.gameObject.SetActive(false);
                EnablePanel(initialCanvas, true);
                GiftBGParent.DOAnchorPosX(-1080f, giftPanelBGMovingDuration).SetEase(giftPanelBGMovingEase);

            }).WaitForCompletion();

        }


    }
    public void OnGiftButton()
    {

        currentGoldCount = 10;
        AsyncOnGiftButton();
        IsClaim = true;
    }
    public void OnGiftRewardad()
    {

        currentGoldCount = 20;
        AsyncOnGiftButton();
        IsClaim = true;
    }
    void AsyncOnGiftButton()
    {
        giftActivetedImage.gameObject.SetActive(false);
        SaveLoadManager.SaveTookTheGold(true);
    }
    public void CreateLinearFlyingSprite(Sprite sprite, Vector2 spriteSize, Vector2 spawnPos, Vector2 targetPos, RectTransform layer, Action onComplete = null)
    {
        CoinBehaviour flyingObj = PoolHandler.I.GetObject<CoinBehaviour>();

        flyingObj.transform.position = spawnPos;
        flyingObj.transform.SetParent(layer);
        flyingObj.GetComponent<RectTransform>().sizeDelta = spriteSize;
        flyingObj.GetComponent<Image>().sprite = sprite;
        Tween flyingTween = TweenHelper.LinearMoveTo(flyingObj.transform, targetPos, onComplete);
        flyingTween.onComplete += () => flyingObj.OnDeactivate();
    }
    public CoinBehaviour CreateCoin(Vector2 spawnPosition, RectTransform layer)
    {
        CoinBehaviour flyingObj = PoolHandler.I.GetObject<CoinBehaviour>();
        flyingObj.transform.position = spawnPosition;
        flyingObj.transform.SetParent(layer);
        flyingObj.GetComponent<RectTransform>().sizeDelta = new Vector2(.4f, .4f);
        flyingObj.transform.DOScale(Vector3.zero, 0f);
        flyingObj.transform.DOScale(new Vector2(250f, 250f), .2f).SetDelay(.1f).SetEase(Ease.OutBack);
        flyingObj.GetComponent<Image>().sprite = CoinSprite;
        return flyingObj;
    }
   
    public async Task GetPrize(Func<int> Count, RectTransform layer, RectTransform target, RectTransform coinSpawnPosition)
    {
        int defaultValue = 100;
        List<CoinBehaviour> lsCoin = new List<CoinBehaviour>();

        Vector2 targetPosition = target.transform.position;
        var delay = 0f;

        lsCoin.Clear();

        for (int i = 0; i < Count(); i++)
        {
            Debug.Log(coinSpawnPosition.position);
            Vector3 pos = coinSpawnPosition.position;
            pos.y += 1f;

            var position = new Vector3(UnityEngine.Random.Range(-.3f, .3f),
                UnityEngine.Random.Range(-.3f, .3f), 0f) + pos;
            CoinBehaviour c = CreateCoin(position, layer);

            var q = new Vector3(UnityEngine.Random.Range(-30f, 30f), UnityEngine.Random.Range(-30f, 30f), 0f);
            c.transform.rotation = Quaternion.Euler(q);
            c.transform.DORotate(Vector3.zero, 0.5f).SetDelay(delay + 0.5f).SetEase(Ease.Flash);
            lsCoin.Add(c);
            delay += 0.1f;
        }

        await Task.Delay(300);

        for (int i = 0; i < lsCoin.Count; i++)
        {
            Tween flyingTween = TweenHelper.CurvingMoveTo(lsCoin[i].transform, targetPosition, () => SaveLoadManager.AddCoin(10f), 1f, .2f, Ease.InOutCubic, Ease.InBack);
            await Task.Delay(100);
        }

        await Task.Delay(defaultValue * Count());

        lsCoin.ForEach(n => n.OnDeactivate());
        lsCoin.Clear();

        await Task.CompletedTask;



    }

    public void Dispose()
    {
        giftButton.onClick.RemoveAllListeners();
        claimButton.onClick.RemoveAllListeners();
        claimRewardedButton.onClick.RemoveAllListeners();
    }
}


//GiftBox Monobehaviour degildir.Bu classta Coruotine baslatmak icin gerekli
public class CoroutineMonoBehaviourParent:PersistentAutoSingleton<CoroutineMonoBehaviourParent>
{

}
