
using System;
using UnityEngine;

public class Pistol : Weapon
{
    public Transform raycastOrigin;
    [SerializeField] ParticleSystem shootParticle;
    [SerializeField] protected float range = 100f;
    int PistolAttackID;

    void Awake()
    {
        PistolAttackID = Animator.StringToHash("Shoot");
    }

    public override void Attack()
    {
        if (Time.time < nextTimeToFire)
            return;

        nextTimeToFire = Time.time + fireRate;

        RaycastHit hit;

        WeaponSFXPlayer?.Play();
        shootParticle?.Play();
        weaponAnimator?.SetTrigger(PistolAttackID);

        if (Physics.Raycast(raycastOrigin.position, raycastOrigin.forward, out hit, range, damagableLayers) )
        {
            IDamagable target = hit.transform.GetComponent<IDamagable>();
            target?.TakeDamage(weaponDamage, hit.point);
        }
    }

}
