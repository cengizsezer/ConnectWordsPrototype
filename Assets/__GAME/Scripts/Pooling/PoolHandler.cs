using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MyBox;
using System.Threading.Tasks;

public sealed class PoolHandler : Singleton<PoolHandler>
{
    [SerializeField] Transform PoolParent;
    public BoardGenerator boardGenerator;
    public WordTableGenerator wordTableGenerator;
    [System.Serializable]
    internal struct Pool
    {
        internal Queue<PoolObject> pooledObjects;
        [MustBeAssigned]
        [SerializeField] internal PoolObject objectPrefab;
        [PositiveValueOnly]
        [SerializeField] internal int poolSize;
    }

    [SerializeField]
    private Pool[] pools;
    
    public TaskCompletionSource<bool> IsLoading = new TaskCompletionSource<bool>();
    private void Start()
    {
        CreatePools();
    }

    void CreatePools()
    {
        for (int i = 0; i < pools.Length; i++)
        {
            pools[i].pooledObjects = new Queue<PoolObject>();

            for (int j = 0; j < pools[i].poolSize; j++)
            {
                PoolObject _po = Instantiate(pools[i].objectPrefab, transform);

                //pools[i].pooledObjects.Enqueue(_po);

                _po.OnCreated();
            }

        }

        
        IsLoading.SetResult(true);
       
    }

    public T GetObject<T>() where T : PoolObject
    {
        for (int i = 0; i < pools.Length; i++)
        {
            if (typeof(T) == pools[i].objectPrefab.GetType())
            {
                if(pools[i].pooledObjects.Count == 0)
                {
                    PoolObject _po = Instantiate(pools[i].objectPrefab, transform);
                    //pools[i].pooledObjects.Enqueue(_po);
                    _po.OnCreated();

                }
                else
                {
                    T t = pools[i].pooledObjects.Dequeue() as T;
                    //pools[i].pooledObjects.Enqueue(t);
                    t.OnSpawn();
                    return t;
                }

               
            }
        }
        return default;
    }

    public void EnqueObject<T>(T po) where T : PoolObject
    {
        for (int i = 0; i < pools.Length; i++)
        {
            if (typeof(T) == pools[i].objectPrefab.GetType())
            {
                pools[i].pooledObjects.Enqueue(po);
            }
        }
    }

}
