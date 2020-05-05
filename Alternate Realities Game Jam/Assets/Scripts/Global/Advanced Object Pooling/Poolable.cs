using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Poolable : MonoBehaviour
{
    [HideInInspector] public int poolableID;
    public bool IsPooled
    {
        get { return isPooled; }
        set
        {
            isPooled = value;
            gameObject.SetActive(!value);
        }
    }

    private bool isPooled;

    public void OnRemoveObject()
    {
        ObjectPoolManager.GetInstance().Remove(this);
    }
}
