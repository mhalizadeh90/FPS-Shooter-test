using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bomber : EnemyAI
{
    [SerializeField] ParticleSystem BombParticle;
    public override void AttackState()
    {
        StandStillAndLookAtPlayer();

        if (!isAlreadyInAttackedState)
        {
            BombParticle.Play();
            AttackAudioPlayer.Play();

            AimAndAttack();
            UpdateNextAttackTimeBasedOnFireRate();
        }
    }

}
