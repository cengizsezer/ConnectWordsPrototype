using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObjects/Main Menu Scene Settings")]
public class MainMenuSceneSettings:ScriptableObject
{
    [Group] [SerializeField] private ShopSceneSettings shopControllerSettings;
    [Group] [SerializeField] private ToggleSceneSettings toggleControllerSettings;
    [Group] [SerializeField] private ChapterPanelControllerSettings chapterControllerSettings;
    [Group] [SerializeField] private GiftPanelControllerSettings giftBoxControllerSettings;

    public ShopSceneSettings ShopControllerSettings => shopControllerSettings;
    public ToggleSceneSettings ToggleControllerSettings => toggleControllerSettings;
    public ChapterPanelControllerSettings ChapterControllerSettings => chapterControllerSettings;
    public GiftPanelControllerSettings GiftBoxControllerSettings => giftBoxControllerSettings;
}
