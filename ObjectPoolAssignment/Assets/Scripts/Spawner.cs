using System.Collections;
using System.Linq;
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
    public float minSpawnRadius = 5;
    public float maxSpawnRadius = 10;
    public float spawningRate;

    #region ObjectPool
    //Dictionary because O(1) (dictionary uses hash)
    [HideInInspector] public static Dictionary<System.Int32, GameObject> enemyObjectPool;

    private float counter;
    private float spawnTimer;
    #endregion

    private void Awake()
    {
        counter = spawningRate;
        if (instance == null)
            instance = this;

        if (RunScriptWithObjectPool)
            InitObjectPool();
    }

    private void InitObjectPool()
    {
        //Initializing with capacity = size;
        enemyObjectPool = new Dictionary<System.Int32, GameObject>(objectPoolSize);

        //Leverage the instance ID that is assigned autmatically to all gameObjects
        for (int i = 0; i < objectPoolSize; i++)
        {
            GameObject enemy = GetEnemyInstance();
            enemy.SetActive(false);
            enemyObjectPool.Add(enemy.GetInstanceID(), enemy);
        }
    }

    private void Update() => Spawn();

    public void Spawn()
    {
        if (RunScriptWithObjectPool)
        {

            SetSpawningRate();

            counter--;

            if (counter <= 0)
            {
                UseObjectPool();
                counter = spawningRate;
            }
        }

        else
        {

            SetSpawningRate();

            counter--;

            if (counter <= 0)
            {
                DontUseObjectPool();
                counter = spawningRate;
            }
        }
    }

    private void SetSpawningRate()
    {
        spawnTimer += Time.deltaTime;

        if (spawnTimer >= 10)
        {
            spawningRate--;

            Debug.Log("New spawning rate: " + spawningRate);

            spawnTimer = 0;
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
        System.Int32 key = -1;

        key = enemyObjectPool.Where(item => item.Value.gameObject.activeSelf == false).FirstOrDefault().Key;

        GameObject enemy;
        if (enemyObjectPool.TryGetValue(key, out enemy))
        {
            enemy = enemyObjectPool[key];
            enemy.transform.position = Random.Range(minSpawnRadius, maxSpawnRadius) * Random.insideUnitCircle.normalized;
            enemy.SetActive(true);
        }
    }

    private GameObject GetEnemyInstance()
    {
        return Instantiate(enemyPrefab, Vector3.zero, Quaternion.identity);
    }
}
