using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using MyBox;
using System.Threading.Tasks;
using System.Linq;

public class LoadingPage : Singleton<LoadingPage>
{
    [SerializeField] bool useImage;
    [ConditionalField(nameof(useImage),false,true)]
    [SerializeField] Image imgLoadingBar;

    [SerializeField] bool useText;
    [ConditionalField(nameof(useText), false, true)]
    [SerializeField] TextMeshProUGUI txtLoadingProgress, txtLoadingProgressMasked;

    delegate void UpdateAsyncVisual(float percentage);
    UpdateAsyncVisual onUpdateAsyncVisual = null;
    [SerializeField] LoadingBarHandler loadingBarHandler;
    private void Start()
    {
        if (useImage)
            onUpdateAsyncVisual += UpdateImage;
        if(useText)
            onUpdateAsyncVisual += UpdateText;
        
        InitGame(() => loadingBarHandler.IsComplate);
    }

    void UpdateImage(float per)
    {
        if(imgLoadingBar != null)
        {
            imgLoadingBar.fillAmount = per;
        }
    }

    private void UpdateText(float per)
    {
        if(txtLoadingProgress != null)
        {
            txtLoadingProgress.text = "Loading " + (per * 100) + "%";
        }
        if (txtLoadingProgressMasked != null)
        {
            txtLoadingProgressMasked.text = "Loading " + (per * 100) + "%";
        }
    }

    private void InitGame(System.Func<bool> res)
    {
        StartCoroutine(LoadRoutine(res));
    }

    float _progression;
   public float Progression
    {
        get => Mathf.Clamp01(_progression);
        set
        {
            float _p = Mathf.Clamp01(value);
            _progression = _p;
            onUpdateAsyncVisual?.Invoke(_progression);

        }
    }
  

    public bool resourceLoaded = false;
    public bool LoadingComplate = false;
    IEnumerator LoadRoutine(System.Func<bool> res)
    {
        GameManager.I.InitResources(true);

        yield return new WaitUntil(()=>resourceLoaded);

        yield return new WaitUntil(()=> LoadingComplate);

        AsyncOperation asyncOperation = SceneManager.LoadSceneAsync(1);
        asyncOperation.allowSceneActivation = false;

        while (!asyncOperation.isDone)
        {
            Progression = .3f + (asyncOperation.progress * .7f);

            if (asyncOperation.progress >= 0.9f)
            {
                Progression = 1f;
                asyncOperation.allowSceneActivation = true;
            }

            yield return null;
        }
    }




}
