using System;
using UnityEngine;

[Serializable]
public class KnockbackEffect : SpellEffect
{
    public float force = 10f;

    public override void Execute(SpellContext ctx)
    {
        if (ctx.Target == null)
            return;

        Rigidbody rb = ctx.Target.GetComponent<Rigidbody>();
        if (rb != null)
        {
            Vector3 dir = (ctx.Target.transform.position - ctx.Caster.transform.position).normalized;
            rb.AddForce(dir * force, ForceMode.Impulse);

            Debug.Log($"Knockback: pushed {ctx.Target.name}");
        }
    }
}
