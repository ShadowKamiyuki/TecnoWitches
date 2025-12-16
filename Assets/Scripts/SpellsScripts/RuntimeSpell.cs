using System;
using UnityEngine;

/// <summary>
/// This class now has runtime behaviour only.
/// </summary>
public class RuntimeSpell
{
    public SpellData data;
    private float cooldownTimer;    // Tiempo restante para poder castear de nuevo
    private float castTimer;        // Tiempo restante de casteo activo
    private bool isCasting;         // Indica si el hechizo está siendo casteado

    public bool IsCasting => isCasting;
    public bool CanCast => !isCasting && cooldownTimer <= 0f; // permite castear si no se esta casteando otro y no hay cooldown activo

    public event Action OnCastStarted;
    public event Action OnCastCompleted;
    public event Action OnCooldownStarted;

    public RuntimeSpell(SpellData data)
    {
        this.data = data;
        cooldownTimer = 0f;
        isCasting = false;
        castTimer = 0f;
    }

    // Tick llamado desde SpellCaster.Update()
    public void Tick(float dt, SpellContext ctx)
    {
        if (cooldownTimer > 0)
            cooldownTimer = Mathf.Max(0, cooldownTimer - dt);

        // Reducir tiempo de cast
        if (isCasting)
        {
            castTimer -= dt;
            if (castTimer <= 0f)
            {
                // Ejecutar efectos al terminar cast
                foreach (var effect in data.effects)
                    effect.Execute(ctx);

                isCasting = false;
                cooldownTimer = data.Cooldown;
            }
        }
    }

    public void StartCast()
    {
        if (!CanCast) return;

        isCasting = true;
        castTimer = data.CastTime;
    }

    public void CancelCast()
    {
        if (!isCasting) return;
        isCasting = false;
        castTimer = 0f;
    }
}
