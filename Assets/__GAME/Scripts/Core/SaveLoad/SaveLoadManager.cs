using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using UnityEngine;

public static class SaveLoadManager
{
    #region GAME

    const string KEY_GAME = "GameASDASDVC";

    public static bool IsFirstGamePlay() => PlayerPrefs.GetInt(KEY_GAME, 1) == 1;
    public static void SetFirstGame(bool isOn)
    {
        PlayerPrefs.SetInt(KEY_GAME, isOn ? 1 : 0);
    }
    #endregion

    #region OVERALL LEVEL

    const string KEY_TOTAL_LEVEL = "jgh311asd";

    public static int GetTotalLevel() => PlayerPrefs.GetInt(KEY_TOTAL_LEVEL, 0);
    public static void SetTotalLevel(int id) {
        PlayerPrefs.SetInt(KEY_TOTAL_LEVEL, id);
        LevelManager.SetLocalLevelIDs();
    }
    public static void IncreaseTotalLevel()
    {
        int id = GetTotalLevel() + 1;

        if(id > GetMaxTotalLevel())
        {
            //if(id > GetMaxTotalLevel())
                IncreasePrizeID();

            SetMaxTotalLevel(id);
        }

        SetTotalLevel(id);
    }

    const string KEY_MAX_TOTAL_LEVEL = "nasd213aa";

    public static int GetMaxTotalLevel() => PlayerPrefs.GetInt(KEY_MAX_TOTAL_LEVEL, 0);
    public static void SetMaxTotalLevel(int id) => PlayerPrefs.SetInt(KEY_MAX_TOTAL_LEVEL, id);

    const string KEY_MAX_TOTAL_CHAPTER = "asdfsdgdfhngnbgfnbv";

    public static int GetMaxTotalChapter() => PlayerPrefs.GetInt(KEY_MAX_TOTAL_CHAPTER, 0);
    public static void SetMaxTotalChapter(int id) => PlayerPrefs.SetInt(KEY_MAX_TOTAL_CHAPTER, id);

    const string KEY_TOTAL_CHAPTER = "wqesdas1323213";

    public static int GetTotalChapter() => PlayerPrefs.GetInt(KEY_TOTAL_CHAPTER, 0);
    public static void SetTotalChapter(int id)
    {
        PlayerPrefs.SetInt(KEY_TOTAL_CHAPTER, id);
    }

    public static void IncreaseTotalChapter()
    {
        int id = GetTotalChapter() + 1;

        if (id > GetMaxTotalChapter())
        {

            SetMaxTotalChapter(id);
        }

        SetTotalChapter(id);
    }




    #endregion

    #region COIN
    const string KEY_COIN = "asda16451da";
    public static float GetCoin() => PlayerPrefs.GetFloat(KEY_COIN,0);
    public static void AddCoin(float add) 
    {
         float current = GetCoin()+add;
        PlayerPrefs.SetFloat(KEY_COIN, current);
        if (GameInUIManager.I!=null&& LevelSuccessUIManager.I!=null&& ChapterFinishUIManager.I!=null)
        {
            GameInUIManager.I.SetCointText();
            LevelSuccessUIManager.I.SetCointText();
            ChapterFinishUIManager.I.SetCointText();
        }
        
         SoundManager.I.PlayOneShot(Sounds.Gold);

        if (MainMenuManager.I!=null)
        {
            MainMenuManager.I.SetCointText();
        }
    }
    
    public static void RemoveCoin(float value)
    {
        float current = GetCoin() - value;
        PlayerPrefs.SetFloat(KEY_COIN, current);
        GameInUIManager.I.SetCointText();
        LevelSuccessUIManager.I.SetCointText();
        ChapterFinishUIManager.I.SetCointText();
        if (MainMenuManager.I != null)
        {
            MainMenuManager.I.SetCointText();
        }
    }


    #endregion

    #region SOUND

    const string KEY_SOUND = "soundsyad";

    public static bool IsSoundAvailable() => PlayerPrefs.GetInt(KEY_SOUND, 1) == 1;
    public static void SetSound(bool isOn)
    {
        PlayerPrefs.SetInt(KEY_SOUND, isOn ? 1 : 0);
        SoundManager.I.SetSoundEnabled(isOn);
    }
#endregion

    #region MUSIC

    const string KEY_MUSIC = "musikac";

    public static bool IsMusicAvailable() => PlayerPrefs.GetInt(KEY_MUSIC, 1) == 1;
    public static void SetMusic(bool isOn)
    {
        PlayerPrefs.SetInt(KEY_MUSIC, isOn ? 1 : 0);
        SoundManager.I.SetMusicEnabled(isOn);
    }


#endregion

    #region HAPTIC

    const string KEY_HAPTIC = "hapidid";

    public static void SetHaptic(bool isOn)
    {
        PlayerPrefs.SetInt(KEY_HAPTIC, isOn ? 1 : 0);
        Vibrator.ChangeVibrationStatus(isOn);
    }

    public static bool IsHapticOn() => PlayerPrefs.GetInt(KEY_HAPTIC, 1) == 1;

#endregion

    #region LANGUAGE

    const string KEY_LANGUAGE = "languages";

    public static string GetLanguageID() => PlayerPrefs.GetString(KEY_LANGUAGE, "EN");
    public static void SetLanguage(string id)
    {
        PlayerPrefs.SetString(KEY_LANGUAGE, id);
        GameManager.I.OnLangChanged();
    }
    #endregion

    #region CHAPTER_ICONS

    const string KEY_CHAPTER_ICONS = "Chapter_Icons";

    public static int GetChapterIconID() => PlayerPrefs.GetInt(KEY_CHAPTER_ICONS, 0);
    public static void SetChapterIcon(int id)
    {
        PlayerPrefs.SetInt(KEY_CHAPTER_ICONS, id);
    }
    #endregion

    #region PRIZE
    const string KEY_PRIZE = "prizeeadaf";

    public static int GetTotalPrizeID()
    {
        return PlayerPrefs.GetInt(KEY_PRIZE, 0);
    }
    static void IncreasePrizeID()
    {
        PlayerPrefs.SetInt(KEY_PRIZE, GetTotalPrizeID() + 1);
        PrizeManager.I.SetCurrentPrize();
    }

    #endregion

    #region DAILY CENGIZ
    //const string KEY_InitialDayWeekYear = "0,0,0";
    //const string KEY_DayWeekYear = "_DayWeekYear";

    //const string KEY_GIVEGOLD = "GIVEGOLDDD";

    //public static void SetGiveGold(bool isOn)
    //{
    //    PlayerPrefs.SetInt(KEY_GIVEGOLD, isOn ? 1 : 0);
    //}

    //public static bool IsGiveGold ()=>PlayerPrefs.GetInt(KEY_GIVEGOLD, 1) == 1;


    //public static int[] GetTime()
    //{
    //    string s_time = PlayerPrefs.GetString(KEY_DayWeekYear, KEY_InitialDayWeekYear);

    //    string[] s_elements = s_time.Split(',');

    //    int[] resault = new int[s_elements.Length];

    //    for (int i = 0; i < s_elements.Length; i++)
    //    {

    //        int _i = -1;

    //        if (int.TryParse(s_elements[i], out _i))
    //        {
    //            resault[i] = _i;
    //        }
    //    }

    //    return resault;
    //}

    //public static void SetTime(int id)
    //{
    //    string s = PlayerPrefs.GetString(KEY_DayWeekYear, KEY_InitialDayWeekYear);
    //    s += "," + id;
    //    PlayerPrefs.SetString(KEY_DayWeekYear, s);

    //}
    #endregion

    #region DAILY AYHAN

    const string KEY_LASTREWARD = "xllsjshaRew";
    const string KEY_DAILYREWARD_STEP = "daststesps12";

    public static DateTime GetLastRewardTime()
    {
        DateTime d = DateTime.UtcNow;
        //d = d.Add(TimeSpan.FromDays(30));
        d = d.Subtract(TimeSpan.FromDays(30));

        string lastTime = PlayerPrefs.GetString(KEY_LASTREWARD, "");

        if (string.IsNullOrEmpty(lastTime))
            return d;

        string format = "MM/dd/yyyy";

        CultureInfo provider = CultureInfo.InvariantCulture;
        if (DateTime.TryParseExact(lastTime, format, provider, DateTimeStyles.None, out DateTime parsedDateTime))
        {
            return parsedDateTime.ToLocalTime();
        }
        else
        {
            Debug.LogError("parsing failed.");
            return d;
        }

    }

    public static void SetLastRewardTime()
    {
        PlayerPrefs.SetString(KEY_LASTREWARD, DailyPrizeManager.dateToday.ToString("MM/dd/yyyy"));
    }

    

    //STEP
    public static int GetDailyRewardStep()
    {
        return (PlayerPrefs.GetInt(KEY_DAILYREWARD_STEP, 0) % 7);
    }

    public static void IncreaseDailyRewardStep()
    {
        int totStep = PlayerPrefs.GetInt(KEY_DAILYREWARD_STEP, 0);
        PlayerPrefs.SetInt(KEY_DAILYREWARD_STEP, totStep + 1);
    }


    const string KEY_GIVEGOLD = "GIVEGOLDDD";

    public static void SaveTookTheGold(bool isOn)
    {
        PlayerPrefs.SetInt(KEY_GIVEGOLD, isOn ? 1 : 0);
    }

    public static bool IsTookTheGold() => PlayerPrefs.GetInt(KEY_GIVEGOLD, 0) == 1;

    #endregion
}
