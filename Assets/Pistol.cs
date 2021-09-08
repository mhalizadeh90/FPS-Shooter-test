
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

    void Awake()
    {
        audio = GetComponent<AudioSource>();
    }

    void Update()
    {
        if(Input.GetButtonDown("Fire1"))
        {
            Shoot();
        }
	}

	private void Shoot()
    {
        RaycastHit hit;

        audio.Play();
        shootParticle.Play();

        if (Physics.Raycast(raycastOrigin.position, raycastOrigin.forward, out hit, range, shootableLayer))
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
