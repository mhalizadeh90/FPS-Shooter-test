using UnityEngine;
using System.Collections;
using UnityEngine.AI;

public class SpwanCollectible : MonoBehaviour
{
    public float SpawnAreaRadius, SpawnMaxHeight;
    public GameObject[] Collectibles;
    public float SpawnDelayMin, SpawnDelayMax;


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
        while (true)
        {
            float delayTime = Random.Range(SpawnDelayMin, SpawnDelayMax);
            yield return new WaitForSeconds(delayTime);
            Instantiate(Collectibles[Random.Range(0, Collectibles.Length)], GetRandomPositionOnMap(), Quaternion.identity);
            print("A Collictable Is Spawned");
        }
    }

    

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawSphere(transform.position, SpawnAreaRadius);
    }
}
