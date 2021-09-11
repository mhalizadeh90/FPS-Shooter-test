using UnityEngine;
using System.Collections;

public class SpwanCollectables : MonoBehaviour
{
    [Header("Spawn Wave Properties")]
    [SerializeField] GameObject[] CollectablesPrefab;
    [SerializeField] float SpawnDelayMin;
    [SerializeField] float SpawnDelayMax;
    [SerializeField] int CollectablesToSpawn;


    [Header("Spawn Position Range")]
    [SerializeField] float SpawnAreaRadius;
    [SerializeField] float SpawnMaxHeight;

    Vector3 GetRandomPositionOnMap()
    {
        Vector3 randomDirection = Random.insideUnitCircle * SpawnAreaRadius;
        randomDirection += transform.position;
        randomDirection.y = SpawnMaxHeight;
        return randomDirection;
    }

    void Start()
    {
        StartCoroutine(SpawnCollectible());
    }

    IEnumerator SpawnCollectible()
    {
        for (int i = 0; i < CollectablesToSpawn; i++)
        {
            float delayTime = Random.Range(SpawnDelayMin, SpawnDelayMax);
            yield return new WaitForSeconds(delayTime);
            //TODO: Replace with Object Pool
            Instantiate(CollectablesPrefab[Random.Range(0, CollectablesPrefab.Length)], GetRandomPositionOnMap(), Quaternion.identity);
        }
    }

    

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawSphere(transform.position, SpawnAreaRadius);
    }
}
