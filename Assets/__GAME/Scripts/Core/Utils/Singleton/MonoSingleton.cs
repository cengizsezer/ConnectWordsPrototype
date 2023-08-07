using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonoSingleton<T> : MonoBehaviour where T : MonoBehaviour
{
    public static T I;

    public virtual void Awake()
    {
        if(I != null)
        {
            Destroy(gameObject);
        }
        else
        {
            I = this as T;
            DontDestroyOnLoad(gameObject);
        }
        
    }


}

