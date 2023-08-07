using System.Collections;
using System.Collections.Generic;
using MyProject.Constant;
using UnityEngine;

namespace MyProject.Settings
{
    [ExecuteAlways]
    public static class SettingsLoader
    {
        static Dictionary<string, SettingsSO> dicSettings;
        static SettingsSO[] settings;
        private static string[] _elementIDs;
        public static string[] ElementIDs
        {
            get
            {
                if (_elementIDs == null)
                {
                    try
                    {
                        LoadSettings();
                    }
                    catch (System.Exception e)
                    {
                        Debug.Log(e);
                    }
                }

                return _elementIDs;
            }

            private set
            {
                _elementIDs = value;
            }
        }

        public static SettingsSO GetSelectedSettingID(string setName)
        {
            SettingsSO sso = null;

            if (dicSettings.TryGetValue(setName, out sso))
            {
                return sso;
            }

            return sso;
        }
        public static void LoadSettings()
        {
            string[] allSettings;

            try
            {
                settings = Resources.LoadAll<SettingsSO>(Constants.Paths.PATH_SETTINGS);

                dicSettings = new Dictionary<string, SettingsSO>();
                allSettings = new string[settings.Length];

                for (int i = 0; i < settings.Length; i++)
                {
                    allSettings[i] = settings[i].name.ToString();
                    dicSettings.Add(settings[i].name.ToString(), settings[i]);
                }

                ElementIDs = allSettings;
            }
            catch (System.Exception e)
            {
                Debug.Log(e);
            }
        }
    }

}
