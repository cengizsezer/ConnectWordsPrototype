using DG.Tweening;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;




namespace MyProject.MainMenu.Settings
{
    [System.Serializable]

    public class ChapterPanelControllerSettings
    {
        [BHeader("Chapter Controller Settings")]
        [SerializeField] float chapterBGMovingDuration;
        [SerializeField] float chapterBGMovingEndValue;
        [SerializeField] Ease chapterBGMovingEase;


        public float ChapterBGMovingDuration => chapterBGMovingDuration;
        public float ChapterBGMovingEndValue => chapterBGMovingEndValue;
        public Ease ChapterBGMovingEase => chapterBGMovingEase;

    }
}


