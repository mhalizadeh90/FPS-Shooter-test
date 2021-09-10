using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bomber : EnemyAI
{
    [SerializeField] ParticleSystem BombParticle;
    public override void Attack()
    {
        StandStillAndLookAtPlayer();

        if (!alreadyAttacked)
        {
            BombParticle.Play();
            ShootSFXPlayer.Play();

            AimAndAttack();
            SetNextAttackTimeBasedOnFireRate();
        }
    }

}
