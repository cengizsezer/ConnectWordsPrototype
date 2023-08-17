using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;



namespace MyProject.MainMenu.References
{
    [Serializable]
    public class SettingsPanelSceneReferences
    {
        [BHeader("Settings Panel Controller References")]
        [SerializeField] CanvasGroup settingsPanel;
        [SerializeField] Button openButton;
        [SerializeField] Button closeButton;
        [SerializeField] CanvasGroup initialCanvas;



        public CanvasGroup InitialCanvas => initialCanvas;
        public CanvasGroup SettingsPanel => settingsPanel;
        public Button OpenButton => openButton;
        public Button CloseButton => closeButton;
    }

}

