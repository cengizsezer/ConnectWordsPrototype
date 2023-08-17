using MyProject.Settings;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

public abstract class GeneratorBase<T>  where T : new()
{
    [HideInInspector]public RectTransform rectTransform;
    [HideInInspector] public GridLayoutGroup gridLayoutGroup;

    protected int cellCount_Vertical;
    protected int cellCount_Horizontal;
    protected float cellSize;
   
    public Transform GetPoolParent() => gridLayoutGroup.transform;
    
    public void EnableGridLayout(bool b) => gridLayoutGroup.enabled = b;

    public bool HasInitialized = false;

    public BoardGenerator BoardGenerator = null;
    public WordTableGenerator WordGenerator = null;
    
    public virtual async Task CreateBoardAsync()
    {
        await Task.Yield();
    }
    
    protected abstract void AdjustScale();





}