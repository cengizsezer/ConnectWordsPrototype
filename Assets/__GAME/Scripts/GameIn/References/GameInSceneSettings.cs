using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObjects/Game In Scene Settings")]
public class GameInSceneSettings:ScriptableObject
{
    [Group] [SerializeField] private WordTableGeneratorSettings wordTableGeneratorSettings;
    [Group] [SerializeField] private BoardGeneratorSettings boardGeneratorSettings;


    public WordTableGeneratorSettings WordTableGeneratorSettings => wordTableGeneratorSettings;
    public BoardGeneratorSettings BoardGeneratorSettings => boardGeneratorSettings;
}
