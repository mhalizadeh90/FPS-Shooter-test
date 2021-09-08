﻿
using System;
using UnityEngine;

public class Pistol : MonoBehaviour
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
    void Awake()
    {
        audio = GetComponent<AudioSource>();
    }

    void Update()
    {
        if(Input.GetButtonDown("Fire1") && Time.time >= nextTimeToFire)
        {
            Shoot();
            nextTimeToFire = Time.time + fireRate;
        }

        if (Input.GetButtonDown("Fire2"))
        {
            Knife();
        }

    }

    private void Knife()
    {
        animator.SetTrigger("Knife");
    }

    private void Shoot()
    {
        RaycastHit hit;

        audio.Play();
        shootParticle.Play();
        animator.SetTrigger("Shoot");
        if (Physics.Raycast(raycastOrigin.position, raycastOrigin.forward, out hit, range, shootableLayer) )
        {
            Debug.Log(hit.transform.name);
            Target target = hit.transform.GetComponent<Target>();
            if (target != null)
            {
                target.TakeDamage(damage);
            }
        }
    }
}
