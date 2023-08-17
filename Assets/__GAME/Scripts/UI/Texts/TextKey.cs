using UnityEngine;
using TMPro;
using UnityEngine.Events;

[RequireComponent(typeof(TextMeshProUGUI))]
public class TextKey : MonoBehaviour
{
    [SerializeField] string key;
    TextMeshProUGUI txt;

    [SerializeField] TextParamGetter[] additionalParams;

    private void Start()
    {
        txt = GetComponent<TextMeshProUGUI>();

        if (!txt) return;

        SetText();
    }

    private void OnEnable()
    {
        LanguageLoader.onLanguageChanged += SetText;
    }

    private void OnDisable()
    {
        LanguageLoader.onLanguageChanged -= SetText;
    }

    public void SetText()
    {
        string val = LanguageLoader.GetValue(key);

        for (int i = 0; i < additionalParams.Length; i++)
        {
            string _res = additionalParams[i].GetResult();

            if (additionalParams[i].seperatorType == SeperatorType.String)
            {
                if (additionalParams[i].position == Position.After)
                    val = string.Join(additionalParams[i].seperator, val, _res);
                else if (additionalParams[i].position == Position.Before)
                    val = string.Join(additionalParams[i].seperator, _res, val);
            }
            else
            {
                if (additionalParams[i].seperatorType == SeperatorType.Enter)
                {
                    if (additionalParams[i].position == Position.After)
                        val = string.Join('\n', val, _res);
                    else if (additionalParams[i].position == Position.Before)
                        val = string.Join('\n', _res, val);
                }
                else if (additionalParams[i].seperatorType == SeperatorType.Tab)
                {
                    if (additionalParams[i].position == Position.After)
                        val = string.Join('\t', val, _res);
                    else if (additionalParams[i].position == Position.Before)
                        val = string.Join('\t', _res, val);
                }
            }


        }

        if (txt != null)
            txt.text = val;
    }

    //public void SetText()
    //{
    //    string val = LanguageLoader.GetValue(key);

    //    for (int i = 0; i < additionalParams.Length; i++)
    //    {
    //        string _res = additionalParams[i].GetResult();

    //        if (additionalParams[i].position == Position.After)
    //            val = string.Join(additionalParams[i].seperator, val, _res);
    //        else if (additionalParams[i].position == Position.Before)
    //            val = string.Join(additionalParams[i].seperator, _res, val);

    //    }

        
    //}

    public void SetKey(string nKey)
    {
        key = nKey;
        SetText();
    }
}
