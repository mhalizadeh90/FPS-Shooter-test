using UnityEngine;
using System;

public class SpeedCollectable : MonoBehaviour,ICollectable
{
    public float Speed;
    public float EffectTime;
    public void Use()
    {
        OnSpeedCollectable?.Invoke(Speed, EffectTime);

        //TODO: Replace with objec pool(in case of scaling up)
        Destroy(gameObject);
    }

    /// <summary>  Arguments: (Speed,EffectTime)  </summary>
    public static Action<float, float> OnSpeedCollectable;
}
