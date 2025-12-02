using UnityEngine;

[CreateAssetMenu(fileName = "ProjectileSpellStrategy", menuName = "TecnoWitches/Spells/Projectile")]
public class ProjectileSpellStrategy : SpellStrategy
{
    [Header("Settings")]
    [SerializeField] private GameObject prefab;

    public override void Cast(SpellContext ctx)
    {
        GameObject projectile = Instantiate(prefab, ctx.firePoint.position, Quaternion.identity);
        ctx.direction.y = 0f;

        Rigidbody rb = projectile.GetComponent<Rigidbody>();
        Debug.Log("Direction: " + ctx.direction);

        if (rb != null)
        {
            rb.velocity = ctx.direction * 10;
        }

        Debug.Log("cast");
        Destroy(projectile, 3f);
    }
}
