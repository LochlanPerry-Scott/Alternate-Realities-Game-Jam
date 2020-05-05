using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPoolManager : SingletonMB<ObjectPoolManager>
{
    private Dictionary<int, ObjectPool> pools = new Dictionary<int, ObjectPool>();

    public Poolable GetObject(Poolable prefab)
    {
        // So we know that the prefab being passed in is unique
        // use to create instances with same data
        if (prefab.poolableID == 0)
            prefab.poolableID = prefab.GetInstanceID();

        int instanceID = prefab.poolableID;

        if (pools.ContainsKey(instanceID))
            return pools[instanceID].GetObject();

        // Used for parenting Objects
        Transform parentGroup = new GameObject(prefab.name + instanceID).transform;
        parentGroup.SetParent(transform);

        // Create the new object pool
        ObjectPool pool = new ObjectPool(prefab, parentGroup);
        pools.Add(instanceID, pool);
        Poolable obj = pool.GetObject();

        return obj;
    }

    /// <summary>
    /// Does the <see cref="pools"/> contain a pool for <paramref name="obj"/>
    /// </summary>
    /// <param name="obj"></param>
    /// <returns></returns>
    public bool PoolObject(Poolable obj)
    {
        if(pools.ContainsKey(obj.poolableID))
        {
            return pools[obj.poolableID].PoolObject(obj);
        }

        Debug.LogWarning("Object isnt pooled");
        return false;
    }

    /// <summary>
    /// Remove <paramref name="obj"/> from the corresponding pool.
    /// </summary>
    /// <param name="obj"></param>
    public void Remove(Poolable obj)
    {
        if (pools.ContainsKey(obj.poolableID))
            pools[obj.poolableID].Remove(obj);
    }
}
