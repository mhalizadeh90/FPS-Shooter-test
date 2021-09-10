using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShotEffect : MonoBehaviour
{

    Animator Animator;
    int TriggerDamageAnimation;
    int TriggerHealAnimation;
    void Awake()
    {
        Animator = GetComponent<Animator>();
        TriggerDamageAnimation = Animator.StringToHash("Shot");
        TriggerHealAnimation = Animator.StringToHash("Heal");
    }

    void OnEnable()
    {
        PlayerHealth.OnPlayerDamaged += ShowDamageEffect;
        PlayerHealth.OnPlayerHealed += ShowHealEffect;
    }

    void ShowDamageEffect(float damage)
    {
        Animator?.SetTrigger(TriggerDamageAnimation);
    }

    void ShowHealEffect(float damage)
    {
        Animator?.SetTrigger(TriggerHealAnimation);
    }


    void OnDisable()
    {
        PlayerHealth.OnPlayerDamaged -= ShowDamageEffect;
        PlayerHealth.OnPlayerHealed -= ShowHealEffect;
    }
}
