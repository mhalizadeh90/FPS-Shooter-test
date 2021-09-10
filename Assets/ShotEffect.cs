using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShotEffect : MonoBehaviour
{

    Animator Animator;
    int TriggerDamageAnimation;
    void Awake()
    {
        Animator = GetComponent<Animator>();
        TriggerDamageAnimation = Animator.StringToHash("Shot");
    }

    void OnEnable()
    {
        PlayerHealth.OnPlayerDamaged += ShowDamageEffect;
    }

    void ShowDamageEffect(float damage)
    {
        Animator?.SetTrigger(TriggerDamageAnimation);
    }

    void OnDisable()
    {
        PlayerHealth.OnPlayerDamaged -= ShowDamageEffect;
    }
}
