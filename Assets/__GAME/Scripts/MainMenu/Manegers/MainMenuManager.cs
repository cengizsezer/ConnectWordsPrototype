using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using TMPro;
using System;
using System.Threading.Tasks;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using System.Collections;

public class MainMenuManager : MenuManager<MainMenuManager>
{
   
    [SerializeField] private Image staticBG;
    [SerializeField] private TextMeshProUGUI txt_Coin;
    [SerializeField] private TextMeshProUGUI main_txtCoin;
    [SerializeField] private MainMenuSceneSettings MainMenuSettings;
    [Group] [SerializeField] private MainMenuSceneReferences MainMenuReferences;
    public MainMenuController MainMenuController = null;

   
    public override void OnStart()
    {
        base.OnStart();

        MainMenuController = new MainMenuController(MainMenuReferences, MainMenuSettings);

        TransitionPanel.I.FadeTransitionImage(false);
        SoundManager.I.PlayMusic();
        
        GameManager.I.staticBG = staticBG;
        staticBG.sprite = GameManager.I.currentSprite;

        SaveLoadManager.SetLastRewardTime();
        SoundManager.I.PlayOneShot(Sounds.Transition, 1f);
        SetCointText();

    }

    public void StartGame(Transform target)
    {
        TweenHelper.ButtonClickAnimation(target,()=>
        {
            TransitionPanel.I.FadeTransitionImage(true, () => StartCoroutine(AsyncLoadScene()));
        });
        
    }

   private IEnumerator AsyncLoadScene()
    {
        var progress = SceneManager.LoadSceneAsync(2, LoadSceneMode.Single);

        while (!progress.isDone)
        {
            yield return null;
        }

        TransitionPanel.I.activeTween.Kill();
        TransitionPanel.I.activeTween = null;

        Scene currentScene = SceneManager.GetActiveScene();
        int sceneIndex = currentScene.buildIndex;

        if (sceneIndex == 2)
        {
            Debug.LogError("cengizzzzzzzzzzzzzzzzzzzzzzzzzzzzzzzzzzzzzzzzzzzz");
        }
    }

    public void SetCointText()
    {
        txt_Coin.text = SaveLoadManager.GetCoin().ToString();
        main_txtCoin.text = SaveLoadManager.GetCoin().ToString();
    }

}
