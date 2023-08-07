using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public class MainMenuController
{

    public MainMenuSceneReferences References { get; private set; }
    public MainMenuSceneSettings Settings { get; private set; }
    // Level Components
    public ShopController ShopController { get; private set; }
    public ToggleController ToggleController { get; private set; }
    public ChapterPanelController ChapterController { get; private set; }
    public SettingsPanelController SettingsPanelController { get; private set; }
    public GiftPanelController GiftBoxPanelController { get; private set; }
    public MainMenuController(MainMenuSceneReferences references, MainMenuSceneSettings settings)
    {
        this.Settings = settings;
        this.References = references;
        CreateMainMenuControllers();
    }

    private void CreateMainMenuControllers()
    {
        ShopController = new ShopController(Settings.ShopControllerSettings, References.ShopControllerSceneReferences);

        ToggleController = new ToggleController(Settings.ToggleControllerSettings, References.ToggleControllerSceneReferences);

        ChapterController = new ChapterPanelController(Settings.ChapterControllerSettings, References.ChapterControllerSceneReferences);

        SettingsPanelController = new SettingsPanelController(References.SettingsControllerSceneReferences);

        GiftBoxPanelController = new GiftPanelController(Settings.GiftBoxControllerSettings, References.GiftPanelControllerSceneReferences);
    }

}
