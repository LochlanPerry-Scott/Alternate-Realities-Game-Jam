using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class ObjectPooling
{
    //public static System.Action OnAddToPool;

    //[System.Serializable]
    //public class PoolData
    //{
    //    public string tag;
    //    public GameObject objectPrefab;
    //}

    //public static List<List<PoolData>> poolDataList;

    ////public static Dictionary<string, Queue<Object>> poolDictionary;
    //private static bool hasInit = false;


    //public static void Init(string _Tag, Object _GameObject, int _Size, Transform _SpawnParent = null)
    //{
    //    if (hasInit)
    //        return;

    //    //poolDictionary = new Dictionary<string, Queue<Object>>();

    //    poolDataList = new List<List<PoolData>>();

    //    Queue<Object> objectPool = new Queue<Object>();

    //    //int itemsInPool = poolDictionary.Count;

    //    for (int i = 0; i < _Size; i++)
    //    {
    //        GameObject obj = Object.Instantiate(_GameObject, (_SpawnParent != null) ? _SpawnParent.transform : null) as GameObject;
    //        OnAddToPool?.Invoke();
    //        obj.SetActive(false);
    //        objectPool.Enqueue(obj);
    //    }

    //    poolDictionary.Add(_Tag, objectPool);
    //    hasInit = true;
    //}

    ////public static void AddBackToPool(string _Tag, GameObject _GameObject, Transform _SpawnParent = null)
    ////{
    ////    Queue<Object> objectPool = new Queue<Object>();

    ////    for (int i = 0; i < _Size; i++)
    ////    {
    ////        GameObject obj = Object.Instantiate(_GameObject, (_SpawnParent != null) ? _SpawnParent.transform : null) as GameObject;
    ////        OnAddToPool?.Invoke();
    ////        obj.SetActive(false);
    ////        objectPool.Enqueue(obj);
    ////    }

    ////    _GameObject.SetActive(false);

    ////    //poolDictionary.Remove(_Tag, objectPool);
    ////}

    //public static void ResetPoolData(string _Tag)
    //{
    //    for (int i = 0; i < poolDictionary.Count; i++)
    //    {
    //        GameObject objectToSpawn = poolDictionary[_Tag].Dequeue() as GameObject;
    //        Debug.Log("Call on Object " + objectToSpawn.name);

    //        objectToSpawn.SetActive(false);
    //        objectToSpawn.transform.position = Vector3.zero;
    //        objectToSpawn.transform.rotation = Quaternion.identity;

    //        poolDictionary[_Tag].Enqueue(objectToSpawn);
    //    }
    //}

    //public static void AddToPool(string _Tag, Object _GameObject, int _Size, Transform _SpawnParent = null)
    //{
    //    Queue<Object> objectPool = new Queue<Object>();

    //    for (int i = 0; i < _Size; i++)
    //    {
    //        GameObject obj = Object.Instantiate(_GameObject, (_SpawnParent != null) ? _SpawnParent.transform : null) as GameObject;
    //        OnAddToPool?.Invoke();
    //        obj.SetActive(false);
    //        objectPool.Enqueue(obj);
    //    }

    //    poolDictionary.Add(_Tag, objectPool);
    //}

    //public static GameObject SpawnFromPool(string tag, Vector3 position, Quaternion rotation, Transform parent = null)
    //{
    //    if(!poolDictionary.ContainsKey(tag))
    //    {
    //        Debug.LogWarning("Pool with " + tag + " Does not exist.");
    //        return null;
    //    }

    //    GameObject objectToSpawn = poolDictionary[tag].Dequeue() as GameObject;

    //    objectToSpawn.SetActive(true);
    //    objectToSpawn.transform.position = position;
    //    objectToSpawn.transform.rotation = rotation;

    //    poolDictionary[tag].Enqueue(objectToSpawn);

    //    return objectToSpawn;
    //}
}
