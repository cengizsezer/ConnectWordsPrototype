using System;
using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

[Serializable]
public class GameInSceneReferences
{
    [Group] [SerializeField] private CurrencyControllerSceneReferences currencyControllerSceneReferences;
    [Group] [SerializeField] private WordTableGeneratorReferences wordTableGeneratorSceneReferences;
    [Group] [SerializeField] private BoardGeneratorSceneReferences boardGeneratorSceneReferences;
    [Group] [SerializeField] private ConnectionControllerSceneReferences connectionControllerSceneReferences;

    public CurrencyControllerSceneReferences CurrencyControllerSceneReferences => currencyControllerSceneReferences;
    public WordTableGeneratorReferences WordTableGeneratorSceneReferences => wordTableGeneratorSceneReferences;
    public BoardGeneratorSceneReferences BoardGeneratorSceneReferences => boardGeneratorSceneReferences;
    public ConnectionControllerSceneReferences ConnectionControllerSceneReferences => connectionControllerSceneReferences;

}
