using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameInManager : Singleton<GameInManager>
{
    [SerializeField] private GameInSceneSettings GameInSettings;

    [Group] [SerializeField] private GameInSceneReferences GameInReferences;

    public GameInController GameInController = null;
   
        

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
