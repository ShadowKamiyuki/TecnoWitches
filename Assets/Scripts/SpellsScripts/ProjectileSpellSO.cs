using UnityEngine;

[CreateAssetMenu(fileName = "ProjectileSpellStrategy", menuName = "TecnoWitches/Spells/Projectile")]
public class ProjectileSpellStrategy : SpellStrategy
{
    [Header("Settings")]
    [SerializeField] private GameObject prefab;

    public override void Cast(SpellContext ctx)
    {
        GameObject projectile = Instantiate(prefab, ctx.firePoint);
        Rigidbody rb = projectile.GetComponent<Rigidbody>();
        rb.velocity = ctx.direction;

        Debug.Log("cast");
        Destroy(projectile, 3f);
    }
}
