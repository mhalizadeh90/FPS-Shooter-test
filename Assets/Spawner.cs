using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    public Wave[] waves;
    public GameObject Enemy;

    Wave currentWave;
    int currentWaveNumber;

    int enemiesReminingToSpawn;
    float nextSpawnTime;

    public Vector3 spawnPositionCenter;
    public Vector3 spawnPositionSize;
    public float spawnSphereSize;
    public Transform[] spawnPositions;


    int remainingEnemy;

    void OnEnable()
    {
        Target.onEnemyDied += CalculateDiedEnemy;
    }

    void CalculateDiedEnemy()
    {
        remainingEnemy--;

        if (remainingEnemy == 0)
            NextWave();
    }
    void Start()
    {
        NextWave();
    }
    void Update()
    {
        if(enemiesReminingToSpawn > 0 && Time.time > nextSpawnTime)
        {
            enemiesReminingToSpawn--;
            nextSpawnTime = Time.time + currentWave.timeBetweenSpawns;

            GameObject spawnedEnemy = Instantiate(Enemy, GetRandomPosition(), Quaternion.identity);
        }
    }

    //TODO: FIND RANDOM LOCATION WITH FILTER based on the distance with player
    private Vector3 GetRandomPosition()
    {
        Vector3 randomPositionToSpawn;
        //randomPositionToSpawn = spawnPositionCenter + new Vector3(UnityEngine.Random.Range(-spawnPositionSize.x / 2, spawnPositionSize.x / 2),
        //    UnityEngine.Random.Range(-spawnPositionSize.y / 2, spawnPositionSize.y / 2),
        //    UnityEngine.Random.Range(-spawnPositionSize.z / 2, spawnPositionSize.z / 2));

        //randomPositionToSpawn = spawnPositionCenter + UnityEngine.Random.insideUnitSphere * spawnSphereSize;
        //randomPositionToSpawn.y = 2;

        // TODO: Check if it is far from player
        randomPositionToSpawn = spawnPositions[UnityEngine.Random.Range(0, spawnPositions.Length)].position;



        return randomPositionToSpawn;
    }

    void NextWave()
    {
        if (currentWaveNumber >= waves.Length)
            return;

        currentWaveNumber++;
        currentWave = waves[currentWaveNumber - 1];

        enemiesReminingToSpawn = currentWave.enemyCount;
        remainingEnemy = enemiesReminingToSpawn;
        
        OnWaveChanged?.Invoke(currentWaveNumber);
    }


    [System.Serializable]
    public class Wave
    {
        public int enemyCount;
        public float timeBetweenSpawns;
    }

    void OnDisable()
    {
        Target.onEnemyDied -= CalculateDiedEnemy;
    }

    public static Action<int> OnWaveChanged;

}
