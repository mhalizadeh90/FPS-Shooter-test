using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class CheatCode : MonoBehaviour
{
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.K)) OnCheatCodeKill?.Invoke();
        if (Input.GetKeyDown(KeyCode.H)) OnCheatCodeInfiniteHealth?.Invoke();
        if (Input.GetKeyDown(KeyCode.I)) OnCheatCodeInfiniteDamage?.Invoke();
    }

    public static Action OnCheatCodeKill;
    public static Action OnCheatCodeInfiniteHealth;
    public static Action OnCheatCodeInfiniteDamage;
}
