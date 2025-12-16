using System;
using UnityEngine;

[Serializable]
public class DamageEffect : SpellEffect
{
    public float damage = 10f;

    public override void Execute(SpellContext ctx)
    {
        if (ctx.target == null)
        {
            Debug.Log("No target for damage effect");
            return;
        }

        Debug.Log($"DamageEffect: {damage} damage to {ctx.target.name}");

        // Ejemplo si tienes un componente de salud:
        // context.Target.GetComponent<Health>()?.TakeDamage(damage);
    }
}
