using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

[Serializable]
public class GiftPanelControllerReferences
{
    [BHeader("Gift Panel Controller References")]
    [SerializeField] CanvasGroup panelGift;
    [SerializeField] CanvasGroup BlackScreen;
    [SerializeField] CanvasGroup canvasGroupClaimButtons;
   
    

    [SerializeField] RectTransform giftBGParent;
    [SerializeField] RectTransform passingPos;
    [SerializeField] RectTransform giftIcon;

    [SerializeField] RectTransform giftPanelSafeArea;
    [SerializeField] RectTransform giftPanelCoinParent;
    [SerializeField] RectTransform target;
    [SerializeField] RectTransform layer;
    
    [SerializeField] Image giftActivetedImage;
    [SerializeField] Sprite coinSprite;

    [SerializeField] Button giftPanelOpenButton;
    [SerializeField] Button claimButton;
    [SerializeField] Button claimRewardedButton;
    [SerializeField] CanvasGroup initialCanvas;



    public CanvasGroup InitialCanvas => initialCanvas;
    public CanvasGroup CanvasGroupClaimButtons => canvasGroupClaimButtons;
    public CanvasGroup BlackScreenCanvas => BlackScreen;

    public CanvasGroup GiftPanel => panelGift;

    public RectTransform GiftBGParent => giftBGParent;
    public RectTransform PassingPosition => passingPos;
    public RectTransform GiftIcon => giftIcon;

    public RectTransform GiftPanelSafeArea => giftPanelSafeArea;
    public RectTransform GiftPanelCoinParent => giftPanelCoinParent;
    public RectTransform GiftPanelCoinTarget => target;
    public RectTransform GiftPanelCoinLayer => layer;
    public Image GiftActivetedImage => giftActivetedImage;
    public Sprite CoinSprite => coinSprite;

    public Button GiftPanelOpenButton => giftPanelOpenButton;

    public Button ClaimButton => claimButton;
    public Button ClaimRewardedButton => claimRewardedButton;
}
