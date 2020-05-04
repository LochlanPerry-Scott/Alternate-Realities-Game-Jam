using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Mathematics;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance
    {
        get;
        private set;
    }
    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(Instance);
        }
        else
        {
            Instance = this;
        }
    }

    [SerializeField] private List<Enemy> enemies = new List<Enemy>();

    public void UpdateWorldTurn()
    {
        foreach (Enemy enemy in enemies)
        {
            enemy.MoveEnemy();
        }
    }
}
