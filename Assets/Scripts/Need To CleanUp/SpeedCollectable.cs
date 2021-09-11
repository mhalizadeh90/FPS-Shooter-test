using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class SpeedCollectable : MonoBehaviour,ICollectable
{
    public float Speed;
    public float EffectTime;
    public void Use()
    {
        OnSpeedCollectable?.Invoke(Speed, EffectTime);
        Destroy(gameObject);
    }

    /// <summary>
    /// Arguments: Speed,EffectTime
    /// </summary>
    public static Action<float, float> OnSpeedCollectable;
}
