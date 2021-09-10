using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Health : MonoBehaviour
{
    public float health = 50f;
    public ParticleSystem DamageParticle;
    public void TakeDamage(float amount, Vector3 DamagePosition)
    {
        health -= amount;
        if(DamageParticle)
        {
            DamageParticle.transform.position = DamagePosition;
            DamageParticle.Play();
        }
        if (health <= 0f)
        {
            Die();
        }
    }

    private void Die()
    {
        //die
        onEnemyDied?.Invoke();
        Destroy(gameObject);
    }

    public static Action onEnemyDied;

}
