using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    [Header("Object Pool toggle on/off")]
    public bool RunScriptWithObjectPool;

    [Header("Parameters")]
    public GameObject enemyPrefab;
    public GameObject playerPrefab;
    public float minSpawnRadius = 10;
    public float maxSpawnRadius = 200;

    private void Update()
    {
        Spawn();
    }

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
        //Where on a line should we spawn?
        float magnitude = Random.Range(minSpawnRadius, maxSpawnRadius);

        //What direction does that line have?
        Vector3 direction = Random.insideUnitCircle.normalized;

        //Create an instance at that position and make it face the player
        Instantiate(enemyPrefab, magnitude * (Quaternion.Euler(new Vector3(Random.Range(0, 360), 0, 0)) * (new Vector3(direction.x, 0, direction.y))), Quaternion.identity);

    }

    private void UseObjectPool()
    {
        throw new System.NotImplementedException();
    }
    
}
