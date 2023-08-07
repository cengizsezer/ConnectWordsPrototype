using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

public class GameInController
{
    public GameInSceneReferences References { get; private set; } = null;
    public GameInSceneSettings Settings { get; private set; } = null;
    // Level Components
    public CurrencyController CurrencyController { get; private set; } = null;
    public PopUpController PopUpController { get; private set; } = null;

    public GeneratorController GeneratorController { get; private set; } = null;

    public ConnectionController ConnectionController { get; set; } = null;

    public GameInController(GameInSceneReferences references, GameInSceneSettings settings)
    {
       
        this.Settings = settings;
        this.References = references;
        CreateGameInControllers();
    }

    private void CreateGameInControllers()
    {
        GeneratorController = new(References.BoardGeneratorSceneReferences, Settings.BoardGeneratorSettings
              , References.WordTableGeneratorSceneReferences, Settings.WordTableGeneratorSettings);
        CurrencyController = new CurrencyController(References.CurrencyControllerSceneReferences);
        PopUpController = new PopUpController();
        
    }

    //Olusturulacak Classlar icin async method
    public async void InitializeGameInControllers()
    {
        await GeneratorController.Initialized();
       ConnectionController = new ConnectionController(References.ConnectionControllerSceneReferences);
       ConnectionController.Controller = GeneratorController;
    }

}
