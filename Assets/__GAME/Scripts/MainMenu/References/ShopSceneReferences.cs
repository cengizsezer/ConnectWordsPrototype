using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;


namespace MyProject.MainMenu.References
{
    [System.Serializable]
    public class ShopSceneReferences
    {
        [BHeader("Shop Controller References")]
        [SerializeField] CanvasGroup panelShop;
        [SerializeField] RectTransform shopBGParent;
        [SerializeField] Button shopCloseButton;
        [SerializeField] Button shopOpenButton;
        [SerializeField] CanvasGroup initialCanvas;



        public CanvasGroup InitialCanvas => initialCanvas;
        public CanvasGroup PanelShop => panelShop;
        public RectTransform ShopBGParent => shopBGParent;
        public Button ShopCloseButton => shopCloseButton;
        public Button ShopOpenButton => shopOpenButton;



    }

}

