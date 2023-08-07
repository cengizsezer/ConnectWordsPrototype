using UnityEngine;


public class ResourceLoader : MonoBehaviour
{
    public static T GetData<T>(string path) where T : Object
    {
        ResourceRequest handle =  LoadObject<T>(path);

        return handle.asset as T;
    }

    public static ResourceRequest LoadObject<T>(string path)
    {
        ResourceRequest req = Resources.LoadAsync(path,typeof(T));

        return req;
    }

}
