using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName ="AI Brain")]
public class AIBrain : ScriptableObject
{
    public float SightRange;
    public float AttackRange;
    public float MaxHealth;
    public float MaxSpeed;
    public float MaxAcceleration;
    [Range(0, 1)] public float MissShotPercentage;
    public float AttackDamage;
}
