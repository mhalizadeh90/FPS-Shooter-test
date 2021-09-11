using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Knife : Weapon
{
    public float knifeAttackRadius = 0.5f;
    int KnifeAttackID;

    void Awake()
    {
        KnifeAttackID = Animator.StringToHash("Knife");
    }


    public override void Attack()
    {
        if (Time.time < nextTimeToFire)
            return;

        nextTimeToFire = Time.time + fireRate;

        weaponAnimator?.SetTrigger(KnifeAttackID);

        Collider[] hits = Physics.OverlapSphere(transform.position, knifeAttackRadius, damagableLayers);
       
        foreach (Collider hit in hits)
        {
            IDamagable target = hit.transform.GetComponent<IDamagable>();
            target?.TakeDamage(weaponDamage, hit.transform.position);
        }
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, knifeAttackRadius);
    }
}
