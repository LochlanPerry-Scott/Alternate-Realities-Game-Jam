using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class ObjectPooling
{
    public static System.Action OnAddToPool;

    [System.Serializable]
    public class ObstaclePool
    {
        public string tag;
        public GameObject prefab;
        public int size = 3;
        public Transform spawnParent;
    }

    public static Dictionary<string, Queue<GameObject>> poolDictionary;

    public static void Init(string _Tag, GameObject _GameObject, int _Size, Transform _SpawnParent = null)
    {
        poolDictionary = new Dictionary<string, Queue<GameObject>>();

        Queue<GameObject> objectPool = new Queue<GameObject>();

        for (int i = 0; i < _Size; i++)
        {
            GameObject obj = Object.Instantiate(_GameObject, (_SpawnParent != null) ? _SpawnParent.transform : _GameObject.transform);
            OnAddToPool?.Invoke();
            obj.SetActive(false);
            objectPool.Enqueue(obj);
        }

        poolDictionary.Add(_Tag, objectPool);
    }

    public static void AddToPool(string _Tag, GameObject _GameObject, int _Size, Transform _SpawnParent = null)
    {
        Queue<GameObject> objectPool = new Queue<GameObject>();

        for (int i = 0; i < _Size; i++)
        {
            GameObject obj = Object.Instantiate(_GameObject, (_SpawnParent != null) ? _SpawnParent.transform : _GameObject.transform);
            OnAddToPool?.Invoke();
            obj.SetActive(false);
            objectPool.Enqueue(obj);
        }

        poolDictionary.Add(_Tag, objectPool);
    }

    public static GameObject SpawnFromPool(string tag, Vector3 position, Quaternion rotation, Transform parent = null)
    {
        if(!poolDictionary.ContainsKey(tag))
        {
            Debug.LogWarning("Pool with " + tag + " Does not exist.");
            return null;
        }

        GameObject objectToSpawn = poolDictionary[tag].Dequeue();

        objectToSpawn.SetActive(true);
        objectToSpawn.transform.position = position;
        objectToSpawn.transform.rotation = rotation;

        poolDictionary[tag].Enqueue(objectToSpawn);

        return objectToSpawn;
    }
}
