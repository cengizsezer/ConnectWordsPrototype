using System;
using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


[Serializable]
public class CurrencyControllerSceneReferences
{
    [BHeader("Currency Controller Settings")]

    [SerializeField] Sprite coinSprite;
    [SerializeField] RectTransform[] targetsRectTransform;
    [SerializeField] RectTransform[] spawnPositions;

    public Sprite CoinSprite => coinSprite;
    public RectTransform[] TargetsRectTransform => targetsRectTransform;
    public RectTransform[] SpawnPositions => spawnPositions;
}
