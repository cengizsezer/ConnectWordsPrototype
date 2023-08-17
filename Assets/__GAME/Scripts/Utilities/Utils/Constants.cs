using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MyProject.Core.Manager;


namespace MyProject.Constant
{
    public static class Constants
    {
        
        public static class Paths
        {
            public const string PATH_LEVELS = "MyProject/Levels/";
            public const string PATH_STRING_ITEMS = "MyProject/Languages/";
            public const string PATH_STRING_ITEMSFOLDER = "MyProject/Languages";
            public const string PATH_LANGUAGE_ICONS = "MyProject/LanguageIcons/";
            public const string PATH_SETTINGSELECTOR = "MyProject/SO/SettingSelector/Selector";
            public const string PATH_SETTINGS = "MyProject/SO/Settings";
            public const string PATH_CHAPTER_ICONS = "MyProject/ChapterIcons/";
            public const string PATH_CHAPTER_BACKGROUNDS = "MyProject/ChapterBG/";
        }


        public static class CoinSettings
        {
            public const float GameCoinValue = 5;
            public const int FirstLevelsCount = 5;
            public const int GameCoinCount = 5;
        }

        public static class AdsSettings
        {
            public const int InterstitialShowLevel = 5;
        }

        public static class BoardSettings
        {
            public const int mixAfter = 5;
        }
        public static class Language
        {
            public static string CURRENT_LANGUAGE => SaveLoadManager.GetLanguageID();
        }
    }
}


