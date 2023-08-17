using MyProject.Constant;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public static class ChapterImageLoader
{

    static Dictionary<string, Sprite> dic_ChapterBackground = new();
    static Dictionary<string, Sprite> dic_ChapterIcon = new();
    
    public static string[] GetAllAvailableChaptersName() => allChapterName;
    static string[] allChapterName;



    #region CHAPTER BACKGROUND
    public static void InitializeAsync(string[] arrChapterName)
    {
        //Debug.Log("***************" + System.Threading.Thread.CurrentThread.ManagedThreadId);
        dic_ChapterBackground = LoadBackgroundAsync(arrChapterName);
        dic_ChapterIcon = LoadIconsAsync(arrChapterName);


    }

    static Dictionary<string, Sprite> LoadBackgroundAsync(string[] arrChapterName)
    {
        Dictionary<string, Sprite> _dic_flags = new Dictionary<string, Sprite>();

        string[] txt = arrChapterName;

        allChapterName = new string[txt.Length];

        for (int i = 0; i < txt.Length; i++)
        {
            allChapterName[i] = txt[i];
           
            _dic_flags.Add(allChapterName[i], LoadSingleBackGround(allChapterName[i]));
            
        }
       
        return _dic_flags;


    }

     static Sprite LoadSingleBackGround(string countryCode)
    {
        Texture2D texture = ResourceLoader.GetData<Texture2D>(Constants.Paths.PATH_CHAPTER_BACKGROUNDS + countryCode);
        
        if (texture != null)
        {
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


    public static Sprite GetBackgroundSprite(string key)
    {
        if (dic_ChapterBackground.TryGetValue(key, out Sprite s))
            return s;
        else
            return null;
    }

    #endregion


    #region CHAPTER ICON


    static Dictionary<string, Sprite> LoadIconsAsync(string[] arrChapterName)
    {
        Dictionary<string, Sprite> _dic_flags = new Dictionary<string, Sprite>();

        string[] txt = arrChapterName;

        allChapterName = new string[txt.Length];

        for (int i = 0; i < txt.Length; i++)
        {
            allChapterName[i] = txt[i];

            _dic_flags.Add(allChapterName[i], LoadSingleIcon(allChapterName[i]));

        }

        return _dic_flags;


    }

    static Sprite LoadSingleIcon(string countryCode)
    {
        Texture2D texture = ResourceLoader.GetData<Texture2D>(Constants.Paths.PATH_CHAPTER_ICONS + countryCode);

        if (texture != null)
        {
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


    public static Sprite GetChapterIcon(string key)
    {
        if (dic_ChapterIcon.TryGetValue(key, out Sprite s))
            return s;
        else
            return null;
    }


    #endregion
}
