using System;
using UnityEngine;

[Serializable]
public class AreaEffect : SpellEffect
{
    public float radius = 3f;

    public override void Execute(SpellContext ctx)
    {
        if (ctx is PositionSpellContext psCtx)
        {
            Collider[] hits = Physics.OverlapSphere(psCtx.TargetPoint, radius);

            foreach (var hit in hits)
            {
                SpellContext targetCtx = new SpellContext(caster: psCtx.Caster, target: hit.gameObject);

                // Aquí solo imprimimos, pero podrías aplicar daño real
                Debug.Log("AOE hit: " + hit.name);
            }
        }
    }
}
