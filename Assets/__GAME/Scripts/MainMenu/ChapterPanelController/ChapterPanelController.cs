using DG.Tweening;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

public class ChapterPanelController:PanelController<ChapterPanelController>,IDisposable
{
    private CanvasGroup panelChapters;
    private RectTransform ChapterBGParent;
    private float ChapterBGMovingDuration;
    private float ChapterBGMovingEndValue;
    private Ease ChapterBGMovingEase;
    private Button openButton;
    private Button closeButton;

    public ChapterPanelController() : this(null, null)
    {
        // diger CTOR u burada cagıracak.
    }

    public ChapterPanelController(ChapterPanelControllerSettings settings, ChapterPanelReferences references)
    {
        this.panelChapters = references.ChapterPanel;
        this.ChapterBGParent = references.ChapterBGParent;
        this.ChapterBGMovingDuration = settings.ChapterBGMovingDuration;
        this.ChapterBGMovingEase = settings.ChapterBGMovingEase;
        this.openButton = references.ChapterOpenButton;
        this.closeButton = references.ChapterCloseButton;
        this.initialCanvas = references.InitialCanvas;
        activePanel = initialCanvas;
        openButton.onClick.RemoveAllListeners();
        closeButton.onClick.RemoveAllListeners();
        openButton.onClick.AddListener(() => OpenChapterPanel(true));
        closeButton.onClick.AddListener(() => CloseChapterPanel(true));

    }
    public void CloseChapterPanel(bool open)
    {
        EnablePanel(initialCanvas, open);
        ChapterBGParent.DOAnchorPosX(-1080f, ChapterBGMovingDuration).SetEase(ChapterBGMovingEase);
    }

    public void Dispose()
    {
        openButton.onClick.RemoveAllListeners();
        closeButton.onClick.RemoveAllListeners();
    }

    public void OpenChapterPanel(bool isOpen)
    {
        EnablePanel(panelChapters, isOpen);
        ChapterBGParent.DOAnchorPosX(0f, ChapterBGMovingDuration).SetEase(ChapterBGMovingEase);
    }


}
