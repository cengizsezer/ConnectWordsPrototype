using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

[Serializable]
public class WordTableGeneratorSettings
{
    [BHeader("WordTable Generator Settings")]

    [SerializeField] float cellScaleOffSet;
    [SerializeField] float clampValue;
    public float CellScaleOffSet => cellScaleOffSet;
    public float ClampValue => clampValue;

}
