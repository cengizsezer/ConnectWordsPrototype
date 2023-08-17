using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;




namespace MyProject.MainMenu.References
{
    [Serializable]
    public class ChapterPanelReferences
    {
        [BHeader("Chapter Controller References")]
        [SerializeField] CanvasGroup panelChapter;
        [SerializeField] RectTransform chapterBGParent;
        [SerializeField] Button chapterCloseButton;
        [SerializeField] Button chapterOpenButton;
        [SerializeField] CanvasGroup initialCanvas;
        public CanvasGroup ChapterPanel => panelChapter;
        public CanvasGroup InitialCanvas => initialCanvas;
        public RectTransform ChapterBGParent => chapterBGParent;
        public Button ChapterCloseButton => chapterCloseButton;
        public Button ChapterOpenButton => chapterOpenButton;

    }

}

