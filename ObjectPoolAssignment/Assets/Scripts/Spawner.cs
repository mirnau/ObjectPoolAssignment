using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    [HideInInspector] public static Spawner instance;
    
    [Header("Object Pool toggle on/off")]
    public bool RunScriptWithObjectPool;
    public int objectPoolSize;

    [Header("Parameters")]
    public GameObject enemyPrefab;
    public GameObject playerPrefab;
    public float minSpawnRadius = 10;
    public float maxSpawnRadius = 200;

    #region ObjectPool
    //Dictionary because O(1) (dictionary uses hash)
    [HideInInspector] public Dictionary<System.Int32, GameObject> objectPool;
    #endregion

    private void Awake()
    {
        if (instance == null)
            instance = this;

        if (RunScriptWithObjectPool)
            InitObjectPool();
    }

    private void InitObjectPool()
    {
        //Initializing with capacity = size;
        objectPool = new Dictionary<System.Int32, GameObject>(objectPoolSize);

        //Leverage the instance ID that is assigned autmatically to all gameObjects
        for (int i = 0; i < objectPoolSize; i++)
        {
            GameObject enemy = GetEnemyInstance();
            enemy.SetActive(false);
            objectPool.Add(enemy.GetInstanceID(), enemy);
        }
    }

    private void Update() => Spawn();

    public void Spawn()
    {
        if (RunScriptWithObjectPool)
        {
            UseObjectPool();
        }
        else
        {
            DontUseObjectPool();
        }
    }

    private void DontUseObjectPool()
    {
        //multiplies the distance (magnitude) from origo, with a random normalized direction vector
        Vector3 position = Random.Range(minSpawnRadius, maxSpawnRadius) * Random.insideUnitCircle.normalized;

        //Create an instance at that position and make it face the player
        Instantiate(enemyPrefab, position, Quaternion.identity);
    }

    private void UseObjectPool()
    {
        throw new System.NotImplementedException();
    }

    private GameObject GetEnemyInstance()
    { 
        return Instantiate(enemyPrefab, Vector3.zero, Quaternion.identity);
    }
}
