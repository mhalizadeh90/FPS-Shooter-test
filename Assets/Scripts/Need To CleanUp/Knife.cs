using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Knife : Weapon
{
    public float damage = 10f;
    public float range = 100f;
    public LayerMask shootableLayer;
    public Transform raycastOrigin;

    AudioSource audio;
    public ParticleSystem shootParticle;
    public Animator animator;

    public float fireRate = 0.15f;
    float nextTimeToFire = 0;

    [Header("Knife")]
    public Transform KnifeSpherePoint;
    public float knifeSphereRadius = 0.5f;
    public float knifeDamage = 5;
    public float knifeRate = 0.15f;
    float nextTimeToKnife = 0;

    void Awake()
    {
        audio = GetComponent<AudioSource>();
    }

    void OnEnable()
    {
        CheatCode.OnCheatCodeInfiniteDamage += setAttackDamageToMaximum;
    }

    void setAttackDamageToMaximum()
    {
        damage = float.MaxValue;
        knifeDamage = float.MaxValue;
    }

    void Update()
    {
        if (Input.GetButtonDown("Fire1") && Time.time >= nextTimeToFire)
        {
            Shoot();
            nextTimeToFire = Time.time + fireRate;
        }

        if (Input.GetButtonDown("Fire2") && Time.time >= nextTimeToKnife)
        {
            Knife();
            nextTimeToKnife = Time.time + knifeRate;
        }

    }

    private void Knife()
    {
        animator.SetTrigger("Knife");

        Collider[] hits = Physics.OverlapSphere(KnifeSpherePoint.position, knifeSphereRadius, shootableLayer);
        foreach (Collider hit in hits)
        {
            AIHealth target = hit.transform.GetComponent<AIHealth>();
            if (target != null)
            {
                target.TakeDamage(knifeDamage, hit.transform.position);
                print("Knife is done");
            }

        }
    }

    private void Shoot()
    {
        RaycastHit hit;

        audio.Play();
        shootParticle.Play();
        animator.SetTrigger("Shoot");
        if (Physics.Raycast(raycastOrigin.position, raycastOrigin.forward, out hit, range, shootableLayer))
        {
            IDamagable EnemyHealth = hit.transform.GetComponent<IDamagable>();
            if (EnemyHealth != null)
            {
                EnemyHealth.TakeDamage(damage, hit.point);
            }
        }
    }

    void OnDisable()
    {
        CheatCode.OnCheatCodeInfiniteDamage -= setAttackDamageToMaximum;
    }
    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(KnifeSpherePoint.position, knifeSphereRadius);
    }
}
