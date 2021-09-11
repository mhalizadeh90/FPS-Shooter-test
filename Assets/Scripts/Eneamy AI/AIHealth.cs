using System;
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

        //TODO: Replace with objec pool(in case of scaling up)
        Destroy(gameObject);
    }

    public static Action onAIDied;

}
