using DG.Tweening;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

[System.Serializable]
public class GiftPanelControllerSettings
{
    [BHeader("Gift Panel Controller Settings")]
    [SerializeField] float giftPanelBGMovingDuration;
    [SerializeField] float giftPanelBGMovingEndValue;
    [SerializeField] Ease giftPanelBGMovingEase;


    public float GiftPanelBGMovingDuration => giftPanelBGMovingDuration;
    public float GiftPanelBGMovingEndValue => giftPanelBGMovingEndValue;
    public Ease GiftPanelBGMovingEase => giftPanelBGMovingEase;

}
