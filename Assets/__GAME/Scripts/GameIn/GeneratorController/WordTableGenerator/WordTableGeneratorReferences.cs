using System;
using UnityEngine;
using UnityEngine.UI;

[Serializable]
public class WordTableGeneratorReferences
{
    [BHeader("WordTable Generator References")]
    [SerializeField] RectTransform rectTransform;
    [SerializeField] GridLayoutGroup gridLayoutGroup;


    public RectTransform RectTransform => rectTransform;
    public GridLayoutGroup GridLayoutGroup => gridLayoutGroup;
}
