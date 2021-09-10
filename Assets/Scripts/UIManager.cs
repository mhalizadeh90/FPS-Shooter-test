using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public Text waveNumber;
    public Text RespawnNumber;
    public GameObject VictoryPanel;
    public GameObject GameOverPanel;

    void Awake()
    {
        VictoryPanel?.SetActive(false);
        GameOverPanel?.SetActive(false);
    }

    void OnEnable()
    {
        Spawner.OnWaveChanged += UpdateWaveNumber;
        Spawner.OnAllEnemiesKilled += ShowVictoryPanel;
        PlayerHealth.OnPlayerDied += ShowGameOverPanel;
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

    void OnDisable()
    {
        Spawner.OnWaveChanged -= UpdateWaveNumber;
        Spawner.OnAllEnemiesKilled -= ShowVictoryPanel;
        PlayerHealth.OnPlayerDied -= ShowGameOverPanel;
    }

    public static Action OnReadyToRespwn;
}
