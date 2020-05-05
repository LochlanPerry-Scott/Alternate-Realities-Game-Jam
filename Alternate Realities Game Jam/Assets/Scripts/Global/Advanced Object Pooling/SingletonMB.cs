using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Singleton used for easy singleton referances to objects of type <see cref="T"/>
/// </summary>
/// <typeparam name="T"></typeparam>
public class SingletonMB<T> : MonoBehaviour where T : SingletonMB<T>
{
    public bool isPresistent;

    public static T instance;

    /// <summary>
    /// Gets the instance of <see cref="T"/>. 
    /// If already exists then instance == already existing instance,
    /// else create a new object to hold the instance
    /// </summary>
    /// <returns></returns>
    public static T GetInstance()
    {
        if(!instance)
        {
            T obj = new GameObject(typeof(T).Name, typeof(T)).GetComponent<T>();
            instance = obj;

            if (obj.isPresistent)
                DontDestroyOnLoad(obj);
        }

        return instance;
    }

    private void Awake()
    {
        if(!instance)
        {
            instance = gameObject.GetComponent<T>();

            if (isPresistent)
                DontDestroyOnLoad(gameObject);
        }
        else
        {
            if (instance != gameObject.GetComponent<T>())
                Destroy(gameObject);
        }
    }
}
