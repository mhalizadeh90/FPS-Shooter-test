using UnityEngine;

public class Collect : MonoBehaviour
{
    [SerializeField] float collectableRadius;
    [SerializeField] LayerMask collectableLayers;
    void Update()
    {
        Collider[] hits = Physics.OverlapSphere(transform.position, collectableRadius, collectableLayers);
        
        if (hits.Length > 0)
        {
            foreach (Collider hit in hits)
            {
                ICollectable collectable = hit.transform.GetComponent<ICollectable>();
                collectable?.Use();
            }
        }

    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, collectableRadius);
    }



}
