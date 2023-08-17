using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public enum ExpandableChildType
{
    Home = 0,
    Music = 1,
    Sound = 2,
    Haptic = 3

}
public class ExpandableChild : MonoBehaviour
{
    [HideInInspector]public Image img;
    [HideInInspector]public RectTransform rectTrans;
    [SerializeField] public TextMeshProUGUI txt;
    public Image img_Active;
    public ExpandableChildType type;

    public int GetID => (int)type;

    void Awake()
    {
        img = GetComponent<Image>();
        rectTrans = GetComponent<RectTransform>();
    }
    
}
