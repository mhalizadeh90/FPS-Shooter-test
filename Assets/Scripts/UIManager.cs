using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    [SerializeField] Text waveNumber;
    [SerializeField] Text RespawnNumber;
    [SerializeField] GameObject VictoryPanel;
    [SerializeField] GameObject GameOverPanel;
    [SerializeField] Slider HealthBarSlider;
    [SerializeField] Animator HealthEffectAnimator;
    int TriggerDamageEffectAnimation;
    int TriggerHealEffectAnimation;

    void Awake()
    {
        VictoryPanel?.SetActive(false);
        GameOverPanel?.SetActive(false);
        TriggerDamageEffectAnimation = Animator.StringToHash("Shot");
        TriggerHealEffectAnimation = Animator.StringToHash("Heal");
    }

    void OnEnable()
    {
        AIWaveSpawner.OnWaveChanged += UpdateWaveNumber;
        AIWaveSpawner.OnAllAIsDied += ShowVictoryPanel;
        PlayerHealth.OnPlayerDied += ShowGameOverPanel;
        PlayerHealth.OnPlayerDamaged += UpdateHealthBar;
        PlayerHealth.OnPlayerHealed += UpdateHealthBar;
        PlayerHealth.OnPlayerDamaged += ShowDamageEffect;
        PlayerHealth.OnPlayerHealed += ShowHealEffect;
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

    void ShowDamageEffect(float damage)
    {
        HealthEffectAnimator?.SetTrigger(TriggerDamageEffectAnimation);
    }

    void ShowHealEffect(float damage)
    {
        HealthEffectAnimator?.SetTrigger(TriggerHealEffectAnimation);
    }

    void OnDisable()
    {
        AIWaveSpawner.OnWaveChanged -= UpdateWaveNumber;
        AIWaveSpawner.OnAllAIsDied -= ShowVictoryPanel;
        PlayerHealth.OnPlayerDied -= ShowGameOverPanel;
        PlayerHealth.OnPlayerDamaged -= UpdateHealthBar;
        PlayerHealth.OnPlayerHealed -= UpdateHealthBar;
        PlayerHealth.OnPlayerDamaged -= ShowDamageEffect;
        PlayerHealth.OnPlayerHealed -= ShowHealEffect;
    }

    public static Action OnReadyToRespwn;
}
