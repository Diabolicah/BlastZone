using Fusion;
using UnityEngine;

public class Health : NetworkBehaviour
{
    [SerializeField] private float defaultMaxHealth = 100f;
    [SerializeField] private float defaultHealthRegenRate = 2f;

    [Networked] public float CurrentHealth { get; set; }
    [Networked] public float MaxHealth { get; set; }
    [Networked] public float HealthRegenRate { get; set; }

    public event System.Action<float, float> OnHealthChanged; // (oldHealth, newHealth)

    private float _lastHealth;

    public override void Spawned()
    {
        if (Object.HasStateAuthority)
        {
            MaxHealth = defaultMaxHealth;
            CurrentHealth = MaxHealth;
            HealthRegenRate = defaultHealthRegenRate;
        }
        _lastHealth = CurrentHealth;
    }

    public override void FixedUpdateNetwork()
    {
        if (Object.HasStateAuthority)
        {
            if (CurrentHealth < MaxHealth)
            {
                float newHealth = Mathf.Min(CurrentHealth + HealthRegenRate * Runner.DeltaTime, MaxHealth);
                if (!Mathf.Approximately(newHealth, CurrentHealth))
                {
                    float oldHealth = CurrentHealth;
                    CurrentHealth = newHealth;
                    OnHealthChanged?.Invoke(oldHealth, newHealth);
                }
                else
                {
                    CurrentHealth = newHealth;
                }
            }
        }
    }

    public void ApplyDamage(float damage)
    {
        if (!Object.HasStateAuthority)
            return;

        float oldHealth = CurrentHealth;
        CurrentHealth = Mathf.Max(CurrentHealth - damage, 0f);
        OnHealthChanged?.Invoke(oldHealth, CurrentHealth);

        if (CurrentHealth <= 0)
        {
            Debug.Log($"{name} has died.");
        }
    }
}
