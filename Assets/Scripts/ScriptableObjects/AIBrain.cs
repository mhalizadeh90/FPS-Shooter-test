using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName ="AI Brain")]
public class AIBrain : ScriptableObject
{
    [Header("SightRange info")]
    public float SightRange;
    public float SightRangeUpdateStep;

    [Header("AttackRange info")]
    public float AttackRange;
    public float AttackRangeUpdateStep;

    [Header("MaxHealth info")]
    public float MaxHealth;
    public float MaxHealthUpdateStep;

    [Header("MaxSpeed info")]
    public float MaxSpeed;
    public float MaxSpeedUpdateStep;

    [Header("MaxAcceleration info")]
    public float MaxAcceleration;
    public float MaxAccelerationUpdateStep;

    [Header("MissShotPercentage info")]
    [Range(0, 1)] public float MissShotPercentage;
    public float MissShotPercentageUpdateStep;

    [Header("AttackDamage info")]
    public float AttackDamage;
    public float AttackDamageUpdateStep;
}
