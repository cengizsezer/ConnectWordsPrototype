using DG.Tweening;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;


[System.Serializable]
public class ShopSceneSettings
{
    [BHeader("Shop Controller Settings")]
    [SerializeField] float shopBGMovingDuration;
    [SerializeField] float shopBGMovingEndValue;
    [SerializeField] Ease shopBGMovingEase;


    public float ShopBGMovingDuration => shopBGMovingDuration;
    public float ShopBGMovingEndValue => shopBGMovingEndValue;
    public Ease ShopBGMovingEase => shopBGMovingEase;

}
