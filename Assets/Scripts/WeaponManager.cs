using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponManager : MonoBehaviour
{
    [SerializeField] Weapon Pistol;
    [SerializeField] Weapon Knife;
    void Update()
    {
        if (Input.GetButtonDown("Fire1")) Pistol.Attack();
        if (Input.GetButtonDown("Fire2")) Knife.Attack();
    }
}
