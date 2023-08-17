using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using MyProject.Core.Manager;

public class LevelButton : MonoBehaviour
{
    [SerializeField] RectTransform rectTransform;
    [SerializeField] Button btn;
    [SerializeField] TextMeshProUGUI txtLevel;
    [SerializeField] Image[] arr_Icons;
    public RectTransform GetRect() => rectTransform;

    int levelID;

    public void Init(int levelID, bool isAvailable,int chapterId)
    {
        this.levelID = levelID;
        //txtLevel.SetText((/*"LEVEL"+ " " +*/levelID + 1).ToString());
        txtLevel.text = string.Join(" ", "LEVEL", levelID + 1).ToString();
        btn.interactable = isAvailable;
        //imgLocked.enabled = !isAvailable;
        var _Icon = ConditionSprite((levelID, chapterId, isAvailable));
        _Icon.gameObject.SetActive(true);
        GetComponent<ExpandableChild>().img_Active = _Icon;
        if (isAvailable)
        {
            btn.onClick.AddListener(() =>
            {
                SaveLoadManager.SetTotalLevel(this.levelID);
                MainMenuManager.I.StartGame(this.transform);
                GameManager.I.currentSprite = ChapterImageLoader.GetBackgroundSprite(LevelManager.GetLevelInfo().chapters[LevelManager.CurrentChapterID].ChapterName);
            });
        }

    }
   
    public Image ConditionSprite((int levelID, int chapterID,bool isAvailable) Level)
    {
        int crnt = SaveLoadManager.GetMaxTotalChapter() * 12 + SaveLoadManager.GetMaxTotalLevel();
        
       
        return Level switch
        {
            (_, _,bool isAvailable) when !isAvailable => arr_Icons[0],//kilitliyse
            (int levelID, int chapterID, bool isAvailable) when chapterID * 12 + levelID < crnt && isAvailable => arr_Icons[1],//levelıd current ıdden kücük ise
            (int levelID, int chapterID, bool isAvailable) when chapterID * 12 + levelID == crnt && isAvailable => arr_Icons[2],//level ıd current ıdye esitse
            _ => arr_Icons[0]
        };
    }

   
}
