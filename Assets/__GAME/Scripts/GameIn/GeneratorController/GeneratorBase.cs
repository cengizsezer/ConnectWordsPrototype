using MyProject.Settings;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

public abstract class GeneratorBase<T>  where T : new()
{
    public RectTransform rectTransform;
    public GridLayoutGroup gridLayoutGroup;

    protected int cellCount_Vertical;
    protected int cellCount_Horizontal;
    protected float cellSize;

    protected List<GeneratorBase<T>> orderedGenerators = new();
    protected abstract int ChildOrder { get; set; }
    public Transform GetPoolParent() => gridLayoutGroup.transform;

    protected void AddGeneratorToOrderList(GeneratorBase<T> generator, int order)
    {
        if (order < 0 || order >= orderedGenerators.Count)
        {
            orderedGenerators.Add(generator);
        }
        else
        {
            orderedGenerators.Insert(order, generator);
        }
    }
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