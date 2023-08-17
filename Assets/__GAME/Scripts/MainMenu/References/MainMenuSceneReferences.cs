using UnityEngine;




namespace MyProject.MainMenu.References
{
    [System.Serializable]
    public class MainMenuSceneReferences
    {
        [Group] [SerializeField] private ShopSceneReferences shopControllerSceneReferences;
        [Group] [SerializeField] private ToggleSceneReferences toggleControllerSceneReferences;
        [Group] [SerializeField] private ChapterPanelReferences chapterControllerSceneReferences;
        [Group] [SerializeField] private GiftPanelControllerReferences giftPanelControllerSceneReferences;
        [Group] [SerializeField] private SettingsPanelSceneReferences settingsControllerSceneReferences;

        public ShopSceneReferences ShopControllerSceneReferences => shopControllerSceneReferences;
        public ToggleSceneReferences ToggleControllerSceneReferences => toggleControllerSceneReferences;
        public ChapterPanelReferences ChapterControllerSceneReferences => chapterControllerSceneReferences;
        public GiftPanelControllerReferences GiftPanelControllerSceneReferences => giftPanelControllerSceneReferences;
        public SettingsPanelSceneReferences SettingsControllerSceneReferences => settingsControllerSceneReferences;

    }

}

