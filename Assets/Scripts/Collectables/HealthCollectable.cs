using UnityEngine;
using System;

public class HealthCollectable : MonoBehaviour,ICollectable
{
    public float HealthAmount = 10;
    public void Use()
    {
        OnHealthCollectableUsed?.Invoke(HealthAmount);
        //TODO: Replace with objec pool(in case of scaling up)
        Destroy(gameObject);
    }

    /// <summary>  Arguments: (HealthAmount)  </summary>
    public static Action<float> OnHealthCollectableUsed;
}
