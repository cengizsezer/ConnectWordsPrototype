using DG.Tweening;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

public class PanelController<T>  where T : new()
{

    protected CanvasGroup initialCanvas;
    public CanvasGroup activePanel;

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
        if (!isEnabled)
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

                if (setAsActive)
                    activePanel = cg;
            }

            onCompleted?.Invoke();
        });
    }


}
