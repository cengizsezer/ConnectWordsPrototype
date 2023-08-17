using System;
using UnityEngine;
using UnityEditor;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


[Serializable]
public class ConnectionControllerSceneReferences
{
    [BHeader("Connection Controller References")]
    [SerializeField] Sprite rightLeftSprite;
    [SerializeField] RectTransform parent;
    [SerializeField] RectTransform ribbon;

    //public Sprite RightLeftSprite => rightLeftSprite;
    public RectTransform Parent => parent;
    public RectTransform Ribbon => ribbon;
    public Sprite RightLeftSprite => rightLeftSprite;
}
