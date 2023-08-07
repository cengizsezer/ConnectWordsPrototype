using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

namespace MyProject.Settings
{
    [ExecuteAlways]
    [CreateAssetMenu(fileName = "New Settings Selector", menuName = "SBF/Settings/New Selector")]
    public class SettingsSelectorSO : ScriptableObject
    {
        public static string[] ElementIDs => SettingsLoader.ElementIDs;

        [ValueDropdown("ElementIDs")]
        public string activeSetting;

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

        //private void OnDestroy()
        //{
        //    SettingsLoader.LoadSettings();
        //}
        //private void OnDisable()
        //{
        //    SettingsLoader.LoadSettings();
        //}
    }

}
