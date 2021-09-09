using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public Text waveNumber;
    public GameObject VictoryPanel;

    void Awake()
    {
        VictoryPanel?.SetActive(false);
    }

    void OnEnable()
    {
        Spawner.OnWaveChanged += UpdateWaveNumber;
        Spawner.OnAllEnemiesKilled += ShowVictoryPanel;
    }

    void UpdateWaveNumber(int wave)
    {
        waveNumber.text = wave.ToString();
    }

    void ShowVictoryPanel()
    {
        VictoryPanel?.SetActive(true);
    }

    void OnDisable()
    {
        Spawner.OnWaveChanged -= UpdateWaveNumber;
        Spawner.OnAllEnemiesKilled -= ShowVictoryPanel;
    }
}
