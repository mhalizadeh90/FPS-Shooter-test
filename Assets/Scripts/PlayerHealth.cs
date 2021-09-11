using System;
using UnityEngine;

public class PlayerHealth : MonoBehaviour, IDamagable
{
    public float MaxHealth = 50f;
    float currentHealth;
    bool isPlayerUndamagable = false;

    void Awake()
    {
        currentHealth = MaxHealth;
    }

    void OnEnable()
    {
        HealthCollectable.OnHealthCollectableUsed += Heal;
        CheatCode.OnCheatCodeInfiniteHealth += SetHealthToInfinite;
        CheatCode.OnCheatCodeKill += PlayerDied;
    }
    public void TakeDamage(float damageAmount, Vector3 damagePosition)
    {
        if (isPlayerUndamagable)
            return;

        currentHealth -= damageAmount;

        // TODO: show some particle at damage position

        if (currentHealth <= 0f) PlayerDied();
        else OnPlayerDamaged?.Invoke(currentHealth/MaxHealth);  // To Show Some Damage Effect
    }

    public void Heal(float amount)
    {
        currentHealth += amount;
        currentHealth = Mathf.Clamp(currentHealth, 0, MaxHealth);
        OnPlayerHealed?.Invoke(currentHealth / MaxHealth);  // To Show Some Heal Effect
    }

    void SetHealthToInfinite()
    {
        Heal(MaxHealth);
        isPlayerUndamagable = true;
    }

    private void PlayerDied()
    {
        OnPlayerDied?.Invoke();
    }

    void OnDisable()
    {
        HealthCollectable.OnHealthCollectableUsed -= Heal;
        CheatCode.OnCheatCodeInfiniteHealth -= SetHealthToInfinite;
        CheatCode.OnCheatCodeKill -= PlayerDied;
    }

    public static Action<float> OnPlayerDamaged;
    public static Action<float> OnPlayerHealed;
    public static Action OnPlayerDied;
}
