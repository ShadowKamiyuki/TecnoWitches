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

        if (ctx is FirePointSpellContext fpCtx)
        {
            GameObject proj = SpawnFromPool(projectilePrefab, fpCtx.FirePoint.position, Quaternion.identity);

            Vector3 direction = new Vector3(fpCtx.Direction.x, 0, fpCtx.Direction.z).normalized;

            if (!proj.TryGetComponent(out ProjectileBehaviour projectile))
            {
                Debug.LogError("El proyectil no tiene ProjectileBehaviour");
                return;
            }

            projectile.Launch(direction, speed);
        }

        Debug.Log("Lanzado proyectil");
    }
}
