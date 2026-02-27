using UnityEngine;
using System;

public class PlayerEnergy : MonoBehaviour
{
    private PlayerStatsRuntime _stats;

    public float CurrentEnergy { get; private set; }

    [SerializeField] private float recoveryPerSecond = 10f;

    public event Action<float, float> OnEnergyChanged;

    public void Initialize(PlayerStatsRuntime stats)
    {
        _stats = stats;
        CurrentEnergy = _stats.MaxHealth.Value; // si luego agregás MaxEnergy, se cambia acá
    }

    private void Update()
    {
        Recover();
    }

    private void Recover()
    {
        if (CurrentEnergy >= _stats.MaxHealth.Value)
            return;

        CurrentEnergy += recoveryPerSecond * Time.deltaTime;
        CurrentEnergy = Mathf.Min(CurrentEnergy, _stats.MaxHealth.Value);

        OnEnergyChanged?.Invoke(CurrentEnergy, _stats.MaxHealth.Value);
    }

    public bool Use(float amount)
    {
        if (CurrentEnergy < amount)
            return false;

        CurrentEnergy -= amount;
        OnEnergyChanged?.Invoke(CurrentEnergy, _stats.MaxHealth.Value);

        return true;
    }
}