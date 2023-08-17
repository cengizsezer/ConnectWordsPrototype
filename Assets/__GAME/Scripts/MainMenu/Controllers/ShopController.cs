using DG.Tweening;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;
using MyProject.MainMenu.Settings;
using MyProject.MainMenu.References;

namespace MyProject.MainMenu.Controllers
{
    public class ShopController : PanelController<ShopController>, IDisposable
    {
        private CanvasGroup panelShop;
        private RectTransform shopBGParent;
        float BGMovingDuration;
        Ease BGMovingEase;
        private Button openButton;
        private Button closeButton;

        public ShopController() : this(null, null)
        {
            // diger CTOR u burada cagıracak.
        }

        public ShopController(ShopSceneSettings settings, ShopSceneReferences references)
        {
            this.panelShop = references.PanelShop;
            this.shopBGParent = references.ShopBGParent;
            this.BGMovingDuration = settings.ShopBGMovingDuration;
            this.BGMovingEase = settings.ShopBGMovingEase;
            this.openButton = references.ShopOpenButton;
            this.closeButton = references.ShopCloseButton;
            this.initialCanvas = references.InitialCanvas;
            activePanel = initialCanvas;
            openButton.onClick.AddListener(() => OpenShopPanel(true));
            closeButton.onClick.AddListener(() => CloseShopPanel(true));

        }


        public void CloseShopPanel(bool open)
        {
            EnablePanel(initialCanvas, open);
            shopBGParent.DOAnchorPosX(-1080f, BGMovingDuration).SetEase(BGMovingEase);
        }



        public void OpenShopPanel(bool isOpen)
        {
            EnablePanel(panelShop, isOpen);
            shopBGParent.DOAnchorPosX(0f, BGMovingDuration).SetEase(BGMovingEase);
        }
        public void Dispose()
        {
            openButton.onClick.RemoveAllListeners();
            closeButton.onClick.RemoveAllListeners();
        }


    }
}

