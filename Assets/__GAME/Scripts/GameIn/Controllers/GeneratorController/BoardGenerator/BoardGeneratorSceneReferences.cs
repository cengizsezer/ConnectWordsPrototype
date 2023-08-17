using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

[Serializable]
public class BoardGeneratorSceneReferences
{
    [BHeader("Board Genenerator References")]
    [SerializeField] RectTransform gridHolder;
    [SerializeField] RectTransform rectTransform;
    [SerializeField] GridLayoutGroup gridLayoutGroup;


    public RectTransform GridHolder => gridHolder;
    public RectTransform RectTransform => rectTransform;
    public GridLayoutGroup GridLayoutGroup => gridLayoutGroup;
}
