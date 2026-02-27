using UnityEngine;
using System;

public class PlayerHealth : MonoBehaviour
{
    private PlayerStatsRuntime _stats;

    public float CurrentHealth { get; private set; }

    [Header("I-Frames")]
    [SerializeField] private float invincibilityDuration = 0.5f;
    private float _invincibilityTimer;
    private bool _isInvincible;

    public event Action<float, float> OnHealthChanged;
    public event Action OnDeath;

    public void Initialize(PlayerStatsRuntime stats)
    {
        _stats = stats;
        CurrentHealth = _stats.MaxHealth.Value;
    }

    private void Update()
    {
        HandleInvincibility();
    }

    private void HandleInvincibility()
    {
        if (!_isInvincible) return;

        _invincibilityTimer -= Time.deltaTime;

        if (_invincibilityTimer <= 0f)
            _isInvincible = false;
    }

    public void TakeDamage(float amount)
    {
        if (_isInvincible) return;

        CurrentHealth -= amount;
        CurrentHealth = Mathf.Max(CurrentHealth, 0f);

        ActivateInvincibility(invincibilityDuration);

        OnHealthChanged?.Invoke(CurrentHealth, _stats.MaxHealth.Value);

        if (CurrentHealth <= 0f)
            OnDeath?.Invoke();
    }

    public void Restore(float amount)
    {
        CurrentHealth += amount;
        CurrentHealth = Mathf.Min(CurrentHealth, _stats.MaxHealth.Value);

        OnHealthChanged?.Invoke(CurrentHealth, _stats.MaxHealth.Value);
    }

    private void ActivateInvincibility(float duration)
    {
        _isInvincible = true;
        _invincibilityTimer = duration;
    }
}