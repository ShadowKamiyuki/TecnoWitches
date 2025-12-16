using System;
using UnityEngine;

[Serializable]
public class ProjectileEffect : SpellEffect
{
    public GameObject projectilePrefab;
    public float speed = 10f;

    public override void Execute(SpellContext ctx)
    {
        if (projectilePrefab == null)
        {
            Debug.LogError("ProjectileEffect: projectilePrefab es null");
            return;
        }

        GameObject proj = SpawnFromPool(projectilePrefab, ctx.caster.transform.position,
        ctx.caster.transform.rotation);

        Vector3 direction = new Vector3(ctx.direction.x, 0, ctx.direction.z).normalized;

        if (!proj.TryGetComponent(out ProjectileBehaviour projectile))
        {
            Debug.LogError("El proyectil no tiene ProjectileBehaviour");
            return;
        }

        projectile.Launch(direction, speed);

        Debug.Log("Lanzado proyectil");
    }
}
