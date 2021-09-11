using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    [SerializeField] protected float weaponDamage = 10f;
    [SerializeField] protected LayerMask damagableLayers;

    [SerializeField] protected AudioSource WeaponSFXPlayer;
    [SerializeField] protected Animator weaponAnimator;

    [SerializeField] protected float fireRate = 0.15f;
    protected float nextTimeToFire = 0;


    void Awake()
    {
        WeaponSFXPlayer = GetComponent<AudioSource>();
    }

    void OnEnable()
    {
        CheatCode.OnCheatCodeInfiniteDamage += setWeaponDamageToMaximum;
    }

    void setWeaponDamageToMaximum()
    {
        weaponDamage = float.MaxValue;
    }


    public virtual void Attack()
    {
        // WEAPON ATTACK BEHAVIOUR
    }

    void OnDisable()
    {
        CheatCode.OnCheatCodeInfiniteDamage -= setWeaponDamageToMaximum;
    }
}
