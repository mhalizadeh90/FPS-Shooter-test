using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIHealth : MonoBehaviour, IDamagable
{
    public float health = 50f;
    public ParticleSystem DamageParticle;

    public void TakeDamage(float damageAmount, Vector3 DamagePosition)
    {
        health -= damageAmount;

        if(DamageParticle)
        {
            DamageParticle.transform.position = DamagePosition;
            DamageParticle.Play();
        }

        if (health <= 0f) AIDie();
    }

    private void AIDie()
    {
        onAIDied?.Invoke();

        //TODO: USE OBJECT POOL INSTEAD OF INSTANTIATE AND DESTROY
        Destroy(gameObject);
    }

    public static Action onAIDied;

}
