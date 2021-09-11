using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    [SerializeField] Text waveNumber;
    [SerializeField] Text RespawnNumber;
    [SerializeField] GameObject VictoryPanel;
    [SerializeField] GameObject GameOverPanel;
    [SerializeField] Slider HealthBarSlider;
   
    void Awake()
    {
        VictoryPanel?.SetActive(false);
        GameOverPanel?.SetActive(false);
    }

    void OnEnable()
    {
        AIWaveSpawner.OnWaveChanged += UpdateWaveNumber;
        AIWaveSpawner.OnAllAIsDied += ShowVictoryPanel;
        PlayerHealth.OnPlayerDied += ShowGameOverPanel;
        PlayerHealth.OnPlayerDamaged += UpdateHealthBar;
        PlayerHealth.OnPlayerHealed += UpdateHealthBar;
    }

    void UpdateWaveNumber(int wave)
    {
        waveNumber.text = wave.ToString();
    }

    void ShowVictoryPanel()
    {
        VictoryPanel?.SetActive(true);
    }

    void ShowGameOverPanel()
    {
        GameOverPanel?.SetActive(true);
        StartCoroutine(CountDownBeforeRespawning());
    }

    IEnumerator CountDownBeforeRespawning()
    {
        for (int i = 3; i >= 0; i--)
        {
            RespawnNumber.text = i.ToString();
            yield return new WaitForSeconds(1);
        }

        OnReadyToRespwn?.Invoke();
    }

    void UpdateHealthBar(float UpdatedHealth)
    {
        HealthBarSlider.value = UpdatedHealth;
    }

    void OnDisable()
    {
        AIWaveSpawner.OnWaveChanged -= UpdateWaveNumber;
        AIWaveSpawner.OnAllAIsDied -= ShowVictoryPanel;
        PlayerHealth.OnPlayerDied -= ShowGameOverPanel;
        PlayerHealth.OnPlayerDamaged -= UpdateHealthBar;
        PlayerHealth.OnPlayerHealed -= UpdateHealthBar;
    }

    public static Action OnReadyToRespwn;
}
