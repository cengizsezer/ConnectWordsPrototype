using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;
using MyProject.MainMenu.References;


namespace MyProject.MainMenu.Controllers
{
    public class SettingsPanelController : PanelController<SettingsPanelController>, IDisposable
    {

        private CanvasGroup panelSettings;
        private Button openButton;
        private Button closeButton;
        public SettingsPanelController() : this(null)
        {

        }



        public SettingsPanelController(SettingsPanelSceneReferences references)
        {
            this.initialCanvas = references.InitialCanvas;
            activePanel = initialCanvas;
            panelSettings = references.SettingsPanel;
            openButton = references.OpenButton;
            closeButton = references.CloseButton;
            openButton.onClick.RemoveAllListeners();
            closeButton.onClick.RemoveAllListeners();
            openButton.onClick.AddListener(() => OpenSettingsPanel(true));
            closeButton.onClick.AddListener(() => CloseSettingsPanel(true));
        }

        public void Dispose()
        {
            openButton.onClick.RemoveAllListeners();
            closeButton.onClick.RemoveAllListeners();
        }

        public void OpenSettingsPanel(bool open)
        {
            EnablePanel(panelSettings, open);
        }

        public void CloseSettingsPanel(bool open)
        {
            EnablePanel(initialCanvas, open);
        }
    }
}

