using System.Collections.Generic;
using UnityEngine;
using MyProject.Constant;
using System.Threading.Tasks;
using static System.Collections.Generic.Dictionary<string, string>;
using Newtonsoft.Json;

public class LanguageLoader
{
    static Dictionary<string, string> dic_crntLanguage;
    static Dictionary<string, Sprite> dic_flags;

    public static string[] GetAllAvailableLanguages() => allLanguages;

    static string[] allLanguages;

    public delegate void OnLanguageChanged();
    public static OnLanguageChanged onLanguageChanged = null;

    public static bool IsInitialized = false;
  
    public static async Task InitializeAsync()
    {
        //Debug.Log("lang await starts");
        dic_crntLanguage = new Dictionary<string, string>();
        dic_crntLanguage = await LoadCurrentLanguage();
        //Debug.Log("lang await ends");

        //Debug.Log("icon await starts");
        dic_flags = LoadIconsAsync();
        //Debug.Log("icon await ends");

        IsInitialized = true;
        //IsAsyncInitialized.SetResult(true);
    }

    #region ICONS

    static Dictionary<string,Sprite> LoadIconsAsync()
    {
        Dictionary<string, Sprite> _dic_flags = new Dictionary<string, Sprite>();
       
        Object[] txt = Resources.LoadAll(Constants.Paths.PATH_STRING_ITEMSFOLDER, typeof(TextAsset));
       
        allLanguages = new string[txt.Length];

        //Debug.Log("ALL LANG COUNT : " + allLanguages.Length);

        for (int i = 0; i < txt.Length; i++)
        {
            allLanguages[i] = txt[i].name;
            //Debug.Log("Language Found : " + allLanguages[i]);
            _dic_flags.Add(allLanguages[i], LoadSingleIcon(allLanguages[i]));
        }
        return _dic_flags;


    }

    static Sprite LoadSingleIcon(string countryCode)
    {
        Texture2D texture = ResourceLoader.GetData<Texture2D>(Constants.Paths.PATH_LANGUAGE_ICONS + countryCode);

        if(texture != null)
        {
            //Debug.Log("ICON FOR " + countryCode + " FOUND");
            Rect rect = new Rect(0, 0, texture.width, texture.height);
            Sprite sprite = Sprite.Create(texture, rect, Vector2.zero);
            return sprite;
        }
        else
        {
            Debug.LogError("ICON FOR " + countryCode + " IS NULL");
        }
        return null;
    }

    #endregion

    static async Task<Dictionary<string, string>> LoadCurrentLanguage()
    {
        return await LoadLanguageAsync();
    }
    static Task<Dictionary<string, string>> LoadLanguageAsync()
    {
        TextAsset txt = ResourceLoader.GetData<TextAsset>(Constants.Paths.PATH_STRING_ITEMS + Constants.Language.CURRENT_LANGUAGE);
        return txt.text.DeserializeJson<Dictionary<string, string>>();
       
    }

    public static Sprite GetFlagIcon(string key)
    {
        if (dic_flags.TryGetValue(key, out Sprite s))
            return s;
        else
            return null;
    }

    public static string GetValue(string key)
    {
        if (dic_crntLanguage.TryGetValue(key, out string s))
            return s;
        else
            return "NULL";
    }
}
