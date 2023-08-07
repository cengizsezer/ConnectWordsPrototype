using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class MenuManager<T> : MonoBehaviour where T : MonoBehaviour
{
    public static T I;

    private void Awake()
    {
        I = this as T;
    }

    [Header("Initial Menu")]
    public CanvasGroup initialCanvas;

    [SerializeField] bool enableAtStart = true;

    CanvasGroup activePanel;

    public virtual void OnStart()
    {
        if (initialCanvas != null && enableAtStart)
            EnablePanel(initialCanvas, true);
    }

    private void Start()
    {
        OnStart();
    }

    public void EnablePanel(CanvasGroup cg, bool isEnabled, System.Action onCompleted = null, bool hideActive = true)
    {

        if (isEnabled && activePanel != null && hideActive)
        {
            FadeInOutPanel(activePanel, false, () => FadeInOutPanel(cg, isEnabled, onCompleted));
        }
        else
        {
            FadeInOutPanel(cg, isEnabled, onCompleted);
        }
    }

    void FadeInOutPanel(CanvasGroup cg, bool isEnabled, System.Action onCompleted = null, bool setAsActive = true)
    {
        if(!isEnabled)
        {
            cg.interactable = false;
            cg.blocksRaycasts = false;
        }

        cg.DOFade(isEnabled ? 1f : 0f, .2f).OnComplete(() =>
        {
            if (isEnabled)
            {
                cg.interactable = true;
                cg.blocksRaycasts = true;

                if(setAsActive)
                    activePanel = cg;
            }

            onCompleted?.Invoke();
        });
    }


}
