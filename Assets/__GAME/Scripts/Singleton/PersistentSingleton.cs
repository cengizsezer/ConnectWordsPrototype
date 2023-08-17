using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

public class PersistentSingleton<T> : MonoBehaviour where T : MonoBehaviour
{
    #region Fields

    /// <summary>
    /// The instance.
    /// </summary>
    protected static T instance;
    protected static bool isDestroyedByGameQuit = false;

    #endregion

    #region Properties

    /// <summary>
    /// Gets the instance.
    /// </summary>
    /// <value>The instance.</value>
    public static T I
    {
        get
        {
            if (isDestroyedByGameQuit) return null;
            if (instance != null) return instance;

            instance = FindObjectOfType<T>();
            if (instance == null)
                instance = new GameObject(typeof(T) + " Instance").AddComponent<T>();

            return instance;
        }
    }

    #endregion

    #region Methods

    protected virtual void Awake()
    {
        if (instance != null)
        {
            Debug.Log($"[{this.GetType().Name}]: Not null, destroy");
            Destroy(gameObject);
            return;
        }
        instance = this as T;
        DontDestroyOnLoad(gameObject);
    }

    protected virtual void OnDestroy()
    {
        if (instance == this) isDestroyedByGameQuit = true;
    }

    #endregion
}
