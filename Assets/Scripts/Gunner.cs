using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gunner : EnemyAI
{
    [SerializeField] ParticleSystem LeftGunParticle;
    [SerializeField] ParticleSystem RightGunParticle;

    public override void AttackState()
    {
        StandStillAndLookAtPlayer();

        if (!isAlreadyInAttackedState)
        {
            PlayGunsParticles();
            AttackAudioPlayer.Play();

            AimAndAttack();

            UpdateNextAttackTimeBasedOnFireRate();
        }
    }

    private void PlayGunsParticles()
    {
        LeftGunParticle.Play();
        RightGunParticle.Play();
    }
}
