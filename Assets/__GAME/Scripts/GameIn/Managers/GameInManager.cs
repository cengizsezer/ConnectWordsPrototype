using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MyProject.MainMenu.Controllers;

public class GameInManager : Singleton<GameInManager>
{
    [SerializeField] private GameInSceneSettings GameInSettings;

    [Group] [SerializeField] private GameInSceneReferences GameInReferences;

    public GameInController GameInController = null;
    public List<RowController> lsGameRowLanet = new();
        

    private void Start()
    {
        GameInController = new GameInController(GameInReferences, GameInSettings);
        EventManager.Send(OnInitialize.Create());

    }

    private void OnEnable()
    {
        EventManager.EventHandlers<OnInitialize>.Register(Initialize);
    }

    private void OnDisable()
    {
        EventManager.EventHandlers<OnInitialize>.Unregister(Initialize);
    }


    public void Initialize(OnInitialize onInitialize)
    {
        GameInController.InitializeGameInControllers();
    }
}
