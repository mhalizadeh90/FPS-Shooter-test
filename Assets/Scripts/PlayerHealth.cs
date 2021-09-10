using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    public float MaxHealth = 50f;
    float health;
    bool isPlayerUndamagable = false;
    void Awake()
    {
        health = MaxHealth;
    }

    void OnEnable()
    {
        HealthCollectable.OnHealthCollectableUsed += Heal;
        CheatCode.OnCheatCodeInfiniteHealth += SetHealthToInfinite;
        CheatCode.OnCheatCodeKill += KillInstantly;

    }
    public void TakeDamage(float amount)
    {
        if (isPlayerUndamagable)
            return;

        health -= amount;
        
        OnPlayerDamaged?.Invoke(health/MaxHealth);
        
        if (health <= 0f)
        {
            Die();
        }
    }

    public void Heal(float amount)
    {
        health += amount;
        health = Mathf.Clamp(health, 0, MaxHealth);
        OnPlayerHealed?.Invoke(health / MaxHealth);
    }

    void SetHealthToInfinite()
    {
        Heal(MaxHealth);
        isPlayerUndamagable = true;
    }

    void KillInstantly()
    {
        isPlayerUndamagable = false;
        TakeDamage(MaxHealth);
    }


    private void Die()
    {
        OnPlayerDied?.Invoke();
    }

    void OnDisable()
    {
        HealthCollectable.OnHealthCollectableUsed -= Heal;
        CheatCode.OnCheatCodeInfiniteHealth -= SetHealthToInfinite;
        CheatCode.OnCheatCodeKill -= KillInstantly;
    }

    public static Action<float> OnPlayerDamaged;
    public static Action<float> OnPlayerHealed;
    public static Action OnPlayerDied;
}
