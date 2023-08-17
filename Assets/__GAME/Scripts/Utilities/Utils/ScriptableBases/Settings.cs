using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MyProject.Constant;
using static MyProject.Settings.SettingsSO;

namespace MyProject.Settings
{
    public static class Settings
    {
        static SettingsSelectorSO selector;

        public static BoardCellSettings BoardCell => GetSettings().boardCellSettings;
        public static ChapterPanelSettings ChapterPanel => GetSettings().chapterPanelSettings;

        public static SettingsSO GetSettings()
        {
            if(selector == null)
            {
                LoadSelector();
                if (selector == null) return null;
            }
            return SettingsLoader.GetSelectedSettingID(selector.activeSetting);
        }

        static void LoadSelector()
        {
            try
            {
                selector = Resources.Load<SettingsSelectorSO>(Constants.Paths.PATH_SETTINGSELECTOR);
            }
            catch(System.Exception e)
            {
                Debug.LogError("Setting Selector Not Found");
                Debug.Log(e);
            }

        }
       
    }

  

}
