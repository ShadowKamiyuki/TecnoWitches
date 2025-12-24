using System;
using UnityEngine;

[Serializable]
public class ConeEffect : SpellEffect
{
    public float radius = 5f;
    [Range(0f, 180f)] public float angle = 45f;

    public override void Execute(SpellContext ctx)
    {
        Vector3 forward = new Vector3(ctx.direction.x, 0, ctx.direction.z).normalized;

        Collider[] hits = Physics.OverlapSphere(ctx.caster.transform.position, radius);

        foreach (Collider hit in hits)
        {
            // Ignoramos al propio caster
            if (hit.transform == ctx.caster) continue;

            // Dirección desde el caster hacia el objetivo, proyectada en XZ
            Vector3 dirToTarget = hit.transform.position - ctx.caster.transform.position;
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
