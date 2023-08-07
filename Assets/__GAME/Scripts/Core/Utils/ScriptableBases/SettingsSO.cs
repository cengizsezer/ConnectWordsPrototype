using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MyBox;

public enum Sizing
{
    FixedPixel,
    Percentage
}

public enum SetType
{
    Min,
    Average,
    Max
}

namespace MyProject.Settings
{
    [ExecuteAlways]
    [CreateAssetMenu(fileName = "new Settings", menuName = "SBF/Settings/New Settings")]
    public class SettingsSO : ScriptableObject
    {
        #region VARIABLES

        public Paddings BoardPaddings;
        public Spacing CellSpaces;
        public BoardCellSettings boardCellSettings;
        public ChapterPanelSettings chapterPanelSettings;

        #endregion

        #region CLASSES

        [System.Serializable]
        public class BoardCellSettings
        {
            public float selectedScale, unselectedScale;
        }

        [System.Serializable]
        public class Spacing
        {
            public Sizing sizingType;

            [ConditionalField(nameof(sizingType), false, Sizing.FixedPixel)]
            public PixelBased pixels;

            [ConditionalField(nameof(sizingType), false, Sizing.Percentage)]
            public PercentageBased percentages;

            [ConditionalField(nameof(sizingType), false, Sizing.Percentage)]
            public bool setEqualXY;

            [ConditionalField(new string[] { nameof(setEqualXY), nameof(sizingType) },new bool[]{false,false},new object[]{true,Sizing.Percentage})]
            public SetType setTo;

            [System.Serializable]
            public struct PixelBased
            {
                public Vector2 space;
            }
            [System.Serializable]
            public struct PercentageBased
            {
                public Vector2 space;
            }
        }

        [System.Serializable]
        public class Paddings
        {
            public Sizing sizingType;

            [ConditionalField(nameof(sizingType), false, Sizing.FixedPixel)]
            public PixelBased pixels;

            [ConditionalField(nameof(sizingType), false, Sizing.Percentage)]
            public PercentageBased percentages;

            [System.Serializable]
            public struct PixelBased
            {
                public int top, bottom, right, left;
            }
            [System.Serializable]
            public struct PercentageBased
            {
                [Range(0f,1f)]
                public float top, bottom, right, left;
            }
        }

        [System.Serializable]
        public class ChapterPanelSettings
        {
            public float chapterAreaCellSpace, chapterAreaExtraSpace;
            public float buttonPanelVerticalPadding, buttonCellSpace, buttonObjectHeight;

        }
        #endregion

        #region MONOBEHAVIOUR FUNCTIONS

        private void OnValidate()
        {
            SettingsLoader.LoadSettings();
        }

        private void OnEnable()
        {
            SettingsLoader.LoadSettings();
        }

        private void Awake()
        {
            SettingsLoader.LoadSettings();
        }

        private void OnDestroy()
        {
            SettingsLoader.LoadSettings();
        }

        private void OnDisable()
        {
            SettingsLoader.LoadSettings();
        }

        #endregion
    }

}

