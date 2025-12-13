using System;
using UnityEngine;

[Serializable]
public class KnockbackEffect : SpellEffect
{
    public float force = 10f;

    public override void Execute(SpellContext ctx)
    {
        if (ctx.target == null)
            return;

        Rigidbody rb = ctx.target.GetComponent<Rigidbody>();
        if (rb != null)
        {
            Vector3 dir = (ctx.target.transform.position - ctx.caster.transform.position).normalized;
            rb.AddForce(dir * force, ForceMode.Impulse);

            Debug.Log($"Knockback: pushed {ctx.target.name}");
        }
    }
}
