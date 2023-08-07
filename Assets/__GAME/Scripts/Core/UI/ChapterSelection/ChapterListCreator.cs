using System.Collections;
using System.Collections.Generic;
using MyProject.Settings;
using UnityEngine;

public class ChapterListCreator : MonoBehaviour
{
    [SerializeField] ExpandableUISystem expandableUISystem;
    [SerializeField] ChapterListElement prefabChapterListElement;
    [SerializeField] RectTransform pool_chapterListelement;
    string[] arrChaptersName;
    List<ChapterListElement> lsChapterElement;
    void Start()
    {
        CreateChapterList();
    }

    void CreateChapterList()
    {
        lsChapterElement = new List<ChapterListElement>();
        LevelManager.LevelJSONInfo info = LevelManager.GetLevelInfo();

        //arrChaptersName = new string[info.chapters.Length];

        float posY = 0;
        float cellSpaceOfLists = Settings.ChapterPanel.chapterAreaCellSpace;
        int sumLevel = 0;
        for (int i = 0; i < info.chapters.Length; i++)
        {
            ChapterListElement cle = Instantiate(prefabChapterListElement, pool_chapterListelement);
            lsChapterElement.Add(cle);
            
            cle.Init(info.chapters[i].ChapterName, i, info.chapters[i].levels.Length, posY, sumLevel);
            posY -= cle.GetTotalHeight();
            posY -= cellSpaceOfLists;
            sumLevel += info.chapters[i].levels.Length;
           
            //arrChaptersName[i] = info.chapters[i].ChapterName;
            expandableUISystem = cle.expandableUISystem.GetComponent<ExpandableUISystem>();
            expandableUISystem.Init(expandableUISystem.transform);
        }

        //ChapterImageLoader.InitializeAsync(arrChaptersName);

        for (int i = 0; i < lsChapterElement.Count; i++)
        {
            lsChapterElement[i].BG.sprite = ChapterImageLoader.GetChapterIcon(LevelManager.arrChaptersName[i]);
        }

        //pool_chapterListelement.rect.Set(0, 0, pool_chapterListelement.rect.width, -posY + Settings.ChapterPanel.chapterAreaExtraSpace);


    }

}
