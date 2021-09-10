using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gunner : EnemyAI
{
    [SerializeField] ParticleSystem LeftGunParticle;
    [SerializeField] ParticleSystem RightGunParticle;

    public override void Attack()
    {
        StandStillAndLookAtPlayer();

        if (!isAlreadyInAttackedState)
        {
            ShowGunsParticles();
            AttackAudioPlayer.Play();

            AimAndAttack();

            SetNextAttackTimeBasedOnFireRate();
        }
    }

    private void ShowGunsParticles()
    {
        LeftGunParticle.Play();
        RightGunParticle.Play();
    }
}
