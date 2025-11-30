using UnityEngine;

public abstract class ProjectileSpellSO : SpellScriptableObject
{
    public ProjectilePool projectilePool;
    public float projectileSpeed = 10f;
    public float damage = 10f;

    public override void Cast(SpellContext ctx)
    {
        if (!CanCast() || projectilePool == null) return;

        RegisterCast();

        GameObject projObj = projectilePool.Pool.Get();
        projObj.transform.position = ctx.firePoint.position;
        projObj.transform.rotation = Quaternion.LookRotation(ctx.direction);

        Projectile proj = projObj.GetComponent<Projectile>();
        if (proj != null)
            proj.Launch(ctx.direction, projectileSpeed, damage, ctx.caster);
    }
}
