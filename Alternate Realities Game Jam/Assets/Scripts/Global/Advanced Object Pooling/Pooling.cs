using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Pooling
{
    /// <summary>
    /// Creates a poolable objects based off <paramref name="obj"/>.
    /// </summary>
    /// <param name="obj"></param>
    /// <param name="fallbaclMethod"></param>
    /// <returns></returns>
    public static GameObject GetObject(GameObject obj, System.Func<GameObject, GameObject> fallbaclMethod)
    {
        // Get the poolable
        Poolable poolable = obj.GetComponent<Poolable>();

        // If the object does not have a poolable referance the call fallback
        if (ReferenceEquals(poolable, null))
            return fallbaclMethod(obj);

        // Else Return the object from the poolable
        return ObjectPoolManager.GetInstance().GetObject(poolable).gameObject;
    }

    public static bool TryPool(GameObject obj, System.Func<GameObject, bool> fallbaclMethod)
    {
        // Get the poolable
        Poolable poolable = obj.GetComponent<Poolable>();

        // If the object does not have a poolable referance the call fallback
        if (ReferenceEquals(poolable, null))
            return fallbaclMethod(obj);

        // Else try pool the poolable
        return ObjectPoolManager.GetInstance().PoolObject(poolable);
    }


    public static bool TryPool(GameObject obj)
    {
        return TryPool(obj, DefaultPoolFallback);
    }

    public static GameObject GetObject(GameObject obj)
    {
        return GetObject(obj, DefaultGetObjectFallback);
    }

    #region Default Fallbacks
    private static bool DefaultPoolFallback(GameObject obj)
    {
        try
        {
            Object.Destroy(obj);
            return true;
        }
        catch
        {
            return false;
        }
    }

    private static GameObject DefaultGetObjectFallback(GameObject obj)
    {
        return Object.Instantiate(obj);
    }
    #endregion
}
