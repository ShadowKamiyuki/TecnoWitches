using System;
using UnityEngine;

[Serializable]
public class AreaEffect : SpellEffect
{
    public float radius = 3f;

    public override void Execute(SpellContext ctx)
    {
        Collider[] hits = Physics.OverlapSphere(ctx.target.transform.position, radius);

        foreach (var hit in hits)
        {
            // Aquí solo imprimimos, pero podrías aplicar daño real
            Debug.Log("AOE hit: " + hit.name);
        }
    }
}
