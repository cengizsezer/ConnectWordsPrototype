using System.Collections;
using System.Collections.Generic;
using MyProject.Constant;
using UnityEngine;
using UnityEngine.UI;

public class SettingMenuManager : MenuManager<SettingMenuManager>
{
    [SerializeField] LanguageButton prefabLangButton;
    [SerializeField] RectTransform poolButton;
    [SerializeField] Image imgLanguageIcon;

    [SerializeField] CanvasGroup panelLanguages;

    public override void OnStart()
    {
        UpdateCurrentLanguageIcon();
        CreateLanguageButtons();
        base.OnStart();
    }

    void CreateLanguageButtons()
    {
        for (int i = 0; i < LanguageLoader.GetAllAvailableLanguages().Length; i++)
        {
            string langCode = LanguageLoader.GetAllAvailableLanguages()[i];
            LanguageButton lb = Instantiate(prefabLangButton, poolButton);
            lb.Init(langCode, LanguageLoader.GetFlagIcon(langCode));
        }
    }

    private void OnEnable()
    {
        LanguageLoader.onLanguageChanged += UpdateCurrentLanguageIcon;
    }

    private void OnDisable()
    {
        LanguageLoader.onLanguageChanged -= UpdateCurrentLanguageIcon;
    }

    void UpdateCurrentLanguageIcon()
    {
        imgLanguageIcon.sprite = LanguageLoader.GetFlagIcon(Constants.Language.CURRENT_LANGUAGE);
    }

    public void OpenLanguagePanel(bool isOpen)
    {
        EnablePanel(panelLanguages, isOpen);
    }

    public void OpenMainPanel(bool isOpen)
    {
        EnablePanel(initialCanvas, isOpen);
    }
}
