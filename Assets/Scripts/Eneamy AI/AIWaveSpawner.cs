using System;
using UnityEngine;

public class AIWaveSpawner : MonoBehaviour
{
    #region Fields

    public EnemyWave[] aiWaves;
    [SerializeField] Transform[] spawnPortals;
    public GameObject[] AIPrefabs;

    EnemyWave currentWave;
    int nextWaveNumber;
    float nextSpawnTime;

    int remainingAIsToSpawn;
    int aliveAIsInThisWave;

    // Variable to check if enemy is not spawning near the player 
    Transform playerPosition;
    const float minimumSpawnDistanceToPlayer = 10;

    #endregion

    void Awake()
    {
        playerPosition = FindObjectOfType<PlayerMovement>().transform;
    }

    void OnEnable()
    {
        AIHealth.onAIDied += UpdateAliveAIsNumber;
    }

    void UpdateAliveAIsNumber()
    {
        aliveAIsInThisWave--;

        if (aliveAIsInThisWave == 0)
        {
            if (nextWaveNumber < aiWaves.Length)
                spawnNextWave();
            else
                OnAllAIsDied?.Invoke();
        }

    }

    void Start()
    {
        spawnNextWave();
    }

    void Update()
    {
        if(remainingAIsToSpawn > 0 && Time.time > nextSpawnTime)
        {
            //TODO: Replace with objec pool(in case of scaling up)
            EnemyAI SpawnedEnemy = Instantiate(AIPrefabs[UnityEngine.Random.Range(0, AIPrefabs.Length)], GetRandomPosition(), Quaternion.identity, transform).GetComponent<EnemyAI>();

            SpawnedEnemy.SetAIBrainBasedOnDificultyLevel(nextWaveNumber-1);

            nextSpawnTime = Time.time + currentWave.timeBetweenSpawns;
            remainingAIsToSpawn--;
        }
    }

    private Vector3 GetRandomPosition()
    {
        Vector3 randomPositionToSpawn;

        // Generate Position and Check If it is far from Current Position of Player
        do
        {
            randomPositionToSpawn = spawnPortals[UnityEngine.Random.Range(0, spawnPortals.Length)].position;
        } 
        while (Vector3.Distance(randomPositionToSpawn, playerPosition.position) <= minimumSpawnDistanceToPlayer);

        return randomPositionToSpawn;
    }

    void spawnNextWave()
    {
        if (nextWaveNumber >= aiWaves.Length)
            return;

        currentWave = aiWaves[nextWaveNumber];

        remainingAIsToSpawn = currentWave.enemyCount;
        aliveAIsInThisWave = remainingAIsToSpawn;
        
        nextWaveNumber++;
        OnWaveChanged?.Invoke(nextWaveNumber);
    }


    void OnDisable()
    {
        AIHealth.onAIDied -= UpdateAliveAIsNumber;
    }

    public static Action<int> OnWaveChanged;
    public static Action OnAllAIsDied;

}
