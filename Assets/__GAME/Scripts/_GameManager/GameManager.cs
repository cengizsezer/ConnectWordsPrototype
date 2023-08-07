using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MyProject.Constant;
//using SBF.Constant;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoSingleton<GameManager>
{
    public Sprite currentSprite;
    public Image staticBG;
    public bool isRunning;
    public bool isFiveLevelCondition = false;


    private void Start()
    {
        if(!SaveLoadManager.IsFirstGamePlay())
           SaveLoadManager.SetFirstGame(true);

    }

    private void OnDestroy()
    {
        if (SaveLoadManager.IsFirstGamePlay())
        {
           //Event
        }
    }

    static Coroutine activeRoutine = null;
    public void InitResources(bool doWithProgresion=false)
    {
        if (activeRoutine != null) return;

        activeRoutine = I.StartCoroutine(InitRoutine(doWithProgresion));
    }

    IEnumerator InitRoutine(bool doWithProgresion=false)
    {
        if(doWithProgresion)
        {
            LoadingPage.I.resourceLoaded = false;
            LoadingPage.I.Progression = 0f;
        }

        if(!LevelManager.IsInitialized)
        {
            ResourceRequest rect = LevelManager.GetLevelRequest();
            yield return new WaitUntil(()=>rect.isDone);
            //Debug.Log("****************************YUKLENDIIIIIIIIIIII");
            string localLevels = (rect.asset as TextAsset).text;
            //Debug.Log("************************** LEVEL JSON :  " + "\n" + localLevels);
            Task t = LevelManager.LoadInfo(localLevels);
            yield return new WaitUntil(() => t.IsCompletedSuccessfully);
            //Debug.Log("*** DESERIALIZE LEVELS FINISHED");
            yield return new WaitForSeconds(.5f);
            LevelManager.OnInitialized();
        }

        if(doWithProgresion)
        {
            LoadingPage.I.Progression = 0.2f;
        }
        
        ChapterImageLoader.InitializeAsync(LevelManager.arrChaptersName);
        currentSprite = ChapterImageLoader.GetBackgroundSprite(LevelManager.GetLevelInfo().chapters[LevelManager.CurrentChapterID].ChapterName);
        staticBG.sprite = currentSprite;
       
        if (!LanguageLoader.IsInitialized)
        {
            yield return LanguageLoader.InitializeAsync();
            yield return new WaitUntil(()=>LanguageLoader.IsInitialized);
            LanguageLoader.onLanguageChanged?.Invoke();
        }

        if (doWithProgresion)
        {
            LoadingPage.I.Progression = 0.4f;
            LoadingPage.I.resourceLoaded = true;

        }

        activeRoutine = null;

    }

    public void OnLangChanged()
    {
        LanguageLoader.IsInitialized = false;
        LevelManager.IsInitialized = false;
        InitResources();
    }
    

    public async Task OnLevelSuccess()
    {
        SaveLoadManager.IncreaseTotalLevel();
        
        if (GameInUIManager.I != null)
        {
            GameInUIManager.I.OnLevelSuccess();
            
        }


        await Task.CompletedTask;
    }
    
}
