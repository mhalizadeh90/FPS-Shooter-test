using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    public float MaxHealth = 50f;
    float health;

    void Awake()
    {
        health = MaxHealth;
    }
    public void TakeDamage(float amount)
    {
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
        OnPlayerDamaged?.Invoke(health / MaxHealth);
    }


    private void Die()
    {
        print("Player is Dead");
        OnPlayerDied?.Invoke();
    }

    public static Action<float> OnPlayerDamaged;
    public static Action OnPlayerDied;
}
