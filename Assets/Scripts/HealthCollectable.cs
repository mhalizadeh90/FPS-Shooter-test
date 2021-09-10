using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class HealthCollectable : MonoBehaviour,ICollectable
{
    public float HealthAmount = 10;
    public void Use()
    {
        OnHealthCollectableUsed?.Invoke(HealthAmount);
        //TODO: REPLACE WITH OBJECT POOL
        Destroy(gameObject);
    }

    public static Action<float> OnHealthCollectableUsed;
}
