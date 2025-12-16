using System;
using UnityEngine;

[Serializable]
public class ConeEffect : SpellEffect
{
    public float radius = 5f;
    public float angle = 45f;

    public override void Execute(SpellContext ctx)
    {
        Vector3 forward = ctx.caster.transform.forward;
        Collider[] hits = Physics.OverlapSphere(ctx.caster.transform.position, radius);

        foreach (var hit in hits)
        {
            Vector3 dir = (hit.transform.position - ctx.caster.transform.position).normalized;
            float dot = Vector3.Dot(forward, dir);

            if (dot > Mathf.Cos(angle * Mathf.Deg2Rad))
            {
                Debug.Log("Cone hit: " + hit.name);
            }
        }
    }
}
