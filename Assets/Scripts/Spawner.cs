using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    public Wave[] waves;
    public GameObject[] Enemies;

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

            EnemyAI SpawnedEnemy = Instantiate(Enemies[UnityEngine.Random.Range(0, Enemies.Length)], GetRandomPosition(), Quaternion.identity).GetComponent<EnemyAI>();

            SetAIDifficulity(SpawnedEnemy);
        }
    }

    private void SetAIDifficulity(EnemyAI spawnedEnemy)
    {
        //TODO: SET AI DIFFICULTY BASED ON THE WAVE
    }

    //TODO: FIND RANDOM LOCATION WITH FILTER based on the distance with player
    private Vector3 GetRandomPosition()
    {
        // TODO: Check if it is far from player
        Vector3 randomPositionToSpawn = spawnPositions[UnityEngine.Random.Range(0, spawnPositions.Length)].position; 
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
