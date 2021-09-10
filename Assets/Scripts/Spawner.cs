using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    public Wave[] waves;
    public GameObject[] Enemies;

    Wave currentWave;
    int nextWaveNumber;

    int enemiesReminingToSpawn;
    float nextSpawnTime;

    public Vector3 spawnPositionCenter;
    public Vector3 spawnPositionSize;
    public float spawnSphereSize;
    public Transform[] spawnPositions;

    Transform PlayerPosition;
    const float minimumSpawnDistanceToPlayer = 10;

    int remainingEnemy;

    void Awake()
    {
        PlayerPosition = FindObjectOfType<PlayerMovement>().transform;
    }

    void OnEnable()
    {
        Health.onEnemyDied += CalculateDiedEnemy;
    }

    void CalculateDiedEnemy()
    {
        remainingEnemy--;

        if (remainingEnemy == 0)
        {
            if (nextWaveNumber < waves.Length)
                NextWave();
            else
                OnAllEnemiesKilled?.Invoke();
        }

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

            //TODO: SET AI DIFFICULTY BASED ON THE WAVE
            SpawnedEnemy.ActivateAIBrainBasedOnDificultyWave(nextWaveNumber-1);
        }
    }

    private Vector3 GetRandomPosition()
    {
        Vector3 randomPositionToSpawn;

        #region Generate Position and Check If it is far from Current Position of Player

        do
        {
            randomPositionToSpawn = spawnPositions[UnityEngine.Random.Range(0, spawnPositions.Length)].position;
        } while (Vector3.Distance(randomPositionToSpawn, PlayerPosition.position) <= minimumSpawnDistanceToPlayer);

        #endregion

        return randomPositionToSpawn;
    }

    void NextWave()
    {
        if (nextWaveNumber >= waves.Length)
            return;

        currentWave = waves[nextWaveNumber];

        enemiesReminingToSpawn = currentWave.enemyCount;
        remainingEnemy = enemiesReminingToSpawn;
        
        nextWaveNumber++;
        OnWaveChanged?.Invoke(nextWaveNumber);
    }


    [System.Serializable]
    public class Wave
    {
        public int enemyCount;
        public float timeBetweenSpawns;
    }

    void OnDisable()
    {
        Health.onEnemyDied -= CalculateDiedEnemy;
    }

    public static Action<int> OnWaveChanged;
    public static Action OnAllEnemiesKilled;

}
