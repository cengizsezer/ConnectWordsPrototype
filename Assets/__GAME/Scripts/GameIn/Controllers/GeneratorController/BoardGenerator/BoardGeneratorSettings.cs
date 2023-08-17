using System;
using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

[Serializable]
public class BoardGeneratorSettings
{
    [BHeader("Board Generator Settings")]
    [SerializeField] float gridHolderOffSet;
    [SerializeField] float clampValue;
    public float GridHolderOffSet => gridHolderOffSet;
    public float ClampValue => clampValue;
}
