using System.Threading.Tasks;
using UnityEngine;
using MyProject.Constant;
using MyProject.Core.Manager;

public class LevelManager
{
    static LevelJSONInfo levelData;
    public static LevelJSONInfo GetLevelInfo() => levelData;
    public static bool IsInitialized = false;
    public static int MaxChapterID { get; private set; }
    public static int ChapterLength { get; private set; }
    public static int CurrentLevelID { get; private set; }
    public static int CurrentChapterID { get; private set; }
    public static bool IsLastLevelOfChapter { get; private set; }
    static Level crntLevel;
    public static Level GetCurrentLevel() => crntLevel;
    public static string[] arrChaptersName;

    public static bool IsFirstLevel()
    {
        int maxLevelID = SaveLoadManager.GetMaxTotalLevel();
        int currentLevel = SaveLoadManager.GetTotalLevel() + 1;
        if (CurrentLevelID == 0 && CurrentChapterID == 0 && maxLevelID < currentLevel)
        {
            return true;
        }


        return false;
    }
    public static bool IsFiveLevel()
    {
        int maxLevelID = SaveLoadManager.GetMaxTotalLevel();
        int currentLevel = SaveLoadManager.GetTotalLevel() + 1;
        
        if(CurrentChapterID == 0 && maxLevelID < currentLevel)
        {
            if(CurrentLevelID == 4)
            {
                return true;
            }
        }
        

        return false;
    }
   
    public static bool AllLevelFinished()
    {
        int lastChapterIndex = MaxChapterID - 1;
        int maxLevelID = SaveLoadManager.GetMaxTotalLevel();
        int currentLevel = SaveLoadManager.GetTotalLevel() + 1;

        if (CurrentChapterID == lastChapterIndex && maxLevelID < currentLevel)
        {
            if (IsLastLevelOfChapter)
            {
                return true;
            }
        }
        return false;
    }
    public static bool InTheFirstFourLevels()
    {
        if(CurrentLevelID <= 3 && CurrentChapterID == 0)
        {
            return true;
        }

        return false;
    }

    public static float LastInterstitialShowTime = 0;
    public static bool IsShowInterstitial()
    {
        Debug.Log(LastInterstitialShowTime);

        if(CurrentChapterID ==0 && CurrentLevelID < Constants.AdsSettings.InterstitialShowLevel  /*&& Application.internetReachability == NetworkReachability.NotReachable*/)
        {
            return false;
        }

        if(CurrentLevelID > Constants.AdsSettings.InterstitialShowLevel)
        {
            if(Time.realtimeSinceStartup - LastInterstitialShowTime >= 60f  /*&& Application.internetReachability != NetworkReachability.NotReachable*/)
            {
                return true;
            }
        }
        return false;
    }
    public static void OnInitialized()
    {
        if (levelData.chapters != null)
        {
            //Debug.Log(levelData.chapters.Length + " *****************************  LOCAL CHAPTERS FOUND");
            MaxChapterID = levelData.chapters.Length;
            //ChapterLength = levelData.chapters[SaveLoadManager.ChapterCurrentID].levels.Length;
            SetLocalLevelIDs();
            IsInitialized = true;
            //IsAsyncInitialized.SetResult(true);

        }
        else
        {
           // Debug.LogError(" *****************************  LOCAL LEVELS COULD NOT BE LOADED");
            IsInitialized = false;
            //IsAsyncInitialized.SetResult(false);
        }
    }
    public async static  Task LoadInfo(string s)
    {
       
        levelData = await s.DeserializeJson<LevelJSONInfo>();
        
    }
    public static  ResourceRequest GetLevelRequest()
    {

        return ResourceLoader.LoadObject<TextAsset>(Constants.Paths.PATH_LEVELS + Constants.Language.CURRENT_LANGUAGE);
    }
    public static void SetLocalLevelIDs(bool isLastLevel = false)
    {
        //Debug.Log("ADJUSTING LOCAL LEVELS");

        IsLastLevelOfChapter = false;

        int totalLevelID = SaveLoadManager.GetTotalLevel();
        int sumLevel = 0;

        bool isLevelFound = false;
        arrChaptersName = new string[levelData.chapters.Length];


        for (int i = 0; i < levelData.chapters.Length; i++)
        {
            arrChaptersName[i] = levelData.chapters[i].ChapterName;
        }
        for (int i = 0; i < levelData.chapters.Length; i++)
        {
            //Debug.Log(i);
            sumLevel += levelData.chapters[i].levels.Length;
            
            if (totalLevelID < sumLevel && !isLevelFound)
            {
                CurrentLevelID = totalLevelID - (sumLevel - levelData.chapters[i].levels.Length);
                CurrentChapterID = i;
                
                ChapterLength = levelData.chapters[i].levels.Length;
                isLevelFound = true;

                if (CurrentLevelID == 0)
                    IsLastLevelOfChapter = true;

                if (isLastLevel)
                    IsLastLevelOfChapter = true;

                //Debug.Log("CRNT CHAPTER ID :" + CurrentChapterID);
                //Debug.Log("CRNT LEVEL ID :" + CurrentLevelID);
                //Debug.Log("CRNT LEVEL LENGTH :" + ChapterLength);
                break;
            }
        }
      
        if (!isLevelFound)
        {
            //Debug.Log("NOT FOUND LOCAL LEVELS... RETRYING");
            SaveLoadManager.SetTotalLevel(sumLevel - 1);
            SetLocalLevelIDs(true);
        }
        else
        {
            //Debug.Log("FOUND LOCAL LEVELS... 1");
            crntLevel = GetLevelByID(CurrentChapterID, CurrentLevelID);
           
            //Debug.Log("FOUND LOCAL LEVELS... 2");
        }
        

    }
    internal static Level GetLevelByID(int chapterID, int levelID)
    {
        
        return levelData.chapters[chapterID].levels[levelID];
    }

    [System.Serializable]
    public struct LevelJSONInfo
    {
        public Chapter[] chapters;
    }

    [System.Serializable]
    public struct Chapter
    {
        public string ChapterName;
        public Level[] levels;
    }

    [System.Serializable]
    public struct Level
    {
        public BoardInfo board;
        public WordTableInfo wordTable;
        public RowInfo row;

        #region Board
        [System.Serializable]
        public class BoardInfo
        {
            public int rowCount;
            public int columnCount;
            public Pairs[] correctPairs;
            public CellInfo[] availableCells;
            public Coloring cellColors;
            public HintLines[] hintCells;
        }

        [System.Serializable]
        public struct Pairs
        {
            public int intID;
            public int charID;
        }
       

        [System.Serializable]
        public class CellInfo
        {
            public int i;
            public int j;
            public int ASCII;
        }
        [System.Serializable]
        public class HintLines
        {
            public HintCellInfo[] cell;
        }

        public class HintCellInfo
        {
            public int i;
            public int j;
        }

        [System.Serializable]
        public class Coloring
        {
            public string[] lineColors;
            public string[] textColors;
            public string hexColorEmptyImage, hexColorEmptyText;
            public int[] colorNumbers;
        }
#endregion
        #region WordTable
        [System.Serializable]
        public class WordTableInfo
        {
            public int rowCount;
            public int columnCount;
            public WordCellInfo[] availableCells;
            public WordColoring wordCellColors;
          
        }
       

        [System.Serializable]
        public class WordCellInfo
        {
            public int i;
            public int j;
            public int ASCII;
        }

        [System.Serializable]
        public class WordColoring
        {
            public string[] colors;
            public string[] textColors;
            public string hexColorEmptyImage, hexColorEmptyText;
            public int[] colorNumbers;
        }



        #endregion
        #region RowInfo

        [System.Serializable]
        public class RowInfo
        {
            public int RowLetterCount;
            public string[] ExtraLetters;
            public Row[] Row;
            public string[] CorrectWords;
        }

        [System.Serializable]
        public class Row
        {
            public LettersInfo[] rowLetters;
        }

        public class LettersInfo
        {
            public int i;
            public int j;
        }


        #endregion
    }
}
