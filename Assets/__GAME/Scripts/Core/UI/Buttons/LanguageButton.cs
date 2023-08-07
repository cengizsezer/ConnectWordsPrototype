using MyProject.Constant;
using MyProject.Settings;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Button))]
public class LanguageButton : MonoBehaviour
{
    [SerializeField] Button btn;
    [SerializeField] Image icon;
    [SerializeField] TextKey txtKey;

    string _langCode;
    public void Init(string langCode,Sprite sprite)
    {
        icon.sprite = sprite;
        _langCode = langCode;

        txtKey.SetKey(langCode);

        btn.onClick.AddListener(() =>
        {
            if(_langCode != Constants.Language.CURRENT_LANGUAGE)
                SaveLoadManager.SetLanguage(_langCode);
        });
    }
}
