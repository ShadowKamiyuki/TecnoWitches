using System;
using UnityEngine;

[Serializable]
public class ConeEffect : SpellEffect
{
    public float radius = 5f;
    [Range(0f, 180f)] public float angle = 45f;

    public override void Execute(SpellContext ctx)
    {
        if (ctx is DirectionalSpellContext dsCtx)
        {
            Vector3 forward = new Vector3(dsCtx.Direction.x, 0, dsCtx.Direction.z).normalized;

            Collider[] hits = Physics.OverlapSphere(dsCtx.Caster.transform.position, radius);

            foreach (Collider hit in hits)
            {
                // Ignoramos al propio caster
                if (hit.transform == dsCtx.Caster) continue;

                // Dirección desde el caster hacia el objetivo, proyectada en XZ
                Vector3 dirToTarget = hit.transform.position - dsCtx.Caster.transform.position;
                Vector3 flatDir = new Vector3(dirToTarget.x, 0, dirToTarget.z).normalized;

                // Calculamos el ángulo entre forward y flatDir
                float flatAngle = Vector3.Angle(forward, flatDir);

                // Si el ángulo está dentro de la mitad del cono, es un hit
                if (flatAngle <= angle / 2f)
                {
                    Debug.Log("Cone hit: " + hit.name);
                }
            }
        }
    }
}
