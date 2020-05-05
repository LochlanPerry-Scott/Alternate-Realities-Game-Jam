using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPool
{
    private List<Poolable> pool = new List<Poolable>();
    private Poolable prefab;
    private Transform parentGroup;

    /// <summary>
    /// Create an instance of <see cref="ObjectPool"/> to assign data.
    /// </summary>
    /// <param name="prefab"></param>
    /// <param name="parentGroup"></param>
    public ObjectPool(Poolable prefab, Transform parentGroup)
    {
        this.prefab = prefab;
        this.parentGroup = parentGroup;
    }

    /// <summary>
    /// Used to get the state of the current Object, 
    /// if object already exists instide the pool then set active value,
    /// else create a new object.
    /// 
    /// The object either created or retrieved from <see cref="pool"/>.
    /// </summary>
    /// <returns></returns>
    public Poolable GetObject()
    {
        Poolable obj;
        int numberItems = pool.Count;
        if(numberItems > 0)
        {
            obj = pool[numberItems - 1];
            obj.IsPooled = false;
            pool.RemoveAt(numberItems - 1);
        }
        else
        {
            obj = Object.Instantiate(prefab);
            obj.transform.parent = parentGroup;
        }

        return obj;
    }

    /// <summary>
    /// Add <paramref name="obj"/> back into the correct pool.
    /// </summary>
    /// <param name="obj"></param>
    /// <returns></returns>
    public bool PoolObject(Poolable obj)
    {
        // Checks if the object is the same as the prefab, ie. poolableID.
        // & the object isnt already pooled
        if(obj.poolableID == prefab.poolableID && !obj.IsPooled)
        {
            pool.Add(obj);
            obj.IsPooled = true;
            return true;
        }

        return false;
    }

    /// <summary>
    /// Remove <paramref name="obj"/> from the pool if it exists in the pool.
    /// </summary>
    /// <param name="obj"></param>
    public void Remove(Poolable obj)
    {
        if (pool.Contains(obj))
            pool.Remove(obj);
    }
}
