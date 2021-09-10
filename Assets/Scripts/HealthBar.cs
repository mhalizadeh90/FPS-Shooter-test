using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    Slider HealthBarSlider;

    void Awake()
    {
        HealthBarSlider = GetComponent<Slider>();
    }
    void OnEnable()
    {
        PlayerHealth.OnPlayerDamaged += UpdateHealthBar;
    }

    void UpdateHealthBar(float UpdatedHealth)
    {
        HealthBarSlider.value = UpdatedHealth;
    }
    void OnDisable()
    {
        PlayerHealth.OnPlayerDamaged += UpdateHealthBar;
    }
}
