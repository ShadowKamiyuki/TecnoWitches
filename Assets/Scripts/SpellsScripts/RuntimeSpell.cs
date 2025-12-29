using System;
using UnityEngine;

/// <summary>
/// Runtime-only spell behavior.
/// Handles casting state, cooldowns and execution timing.
/// </summary>
public class RuntimeSpell
{
    public SpellData Data { get; }
    public SpellContextType ContextType => Data.contextType;

    private float cooldownTimer;    // Tiempo restante para poder castear de nuevo
    private float castTimer;        // Tiempo restante de casteo activo
    private bool isCasting;         // Indica si el hechizo esta siendo casteado

    private SpellContext castContext; // contexto capturado al iniciar el cast

    public bool IsCasting => isCasting;
    public bool CanCast => !isCasting && cooldownTimer <= 0f; // permite castear si no se esta casteando otro y no hay cooldown activo

    public event Action OnCastStarted;
    public event Action OnCastCompleted;
    public event Action OnCooldownStarted;

    public RuntimeSpell(SpellData data)
    {
        Data = data;
    }

    // Tick llamado desde SpellCaster.Update()
    public void Tick(float dt)
    {
        if (cooldownTimer > 0)
            cooldownTimer = Mathf.Max(0, cooldownTimer - dt);

        if (!isCasting)
            return;

        castTimer -= dt;
        if (castTimer > 0f)
            return;

        CompleteCast();
    }

    public void StartCast(SpellContext ctx)
    {
        if (!CanCast)
            return;

        castContext = ctx;
        castTimer = Data.CastTime;
        isCasting = true;

        OnCastStarted?.Invoke();

        if (castTimer <= 0f)
            CompleteCast();
    }

    private void CompleteCast()
    {
        foreach (var effect in Data.effects)
            effect.Execute(castContext);

        isCasting = false;
        castContext = null;

        cooldownTimer = Data.Cooldown;
        OnCastCompleted?.Invoke();
        OnCooldownStarted?.Invoke();
    }

    public void CancelCast()
    {
        if (!isCasting)
            return;

        isCasting = false;
        castTimer = 0f;
        castContext = null;
    }
}
