using UnityEngine;

public class Projectile : MonoBehaviour
{
    private PooledProjectile pooled;
    private float speed;
    private float damage;
    private GameObject caster;

    private void Awake()
    {
        pooled = GetComponent<PooledProjectile>();
    }

    public void Launch(Vector3 direction, float speed, float damage, GameObject caster)
    {
        this.speed = speed;
        this.damage = damage;
        this.caster = caster;

        Rigidbody rb = GetComponent<Rigidbody>();
        if (rb != null)
        {
            rb.velocity = direction.normalized * speed;
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        // Aquí podrías manejar daño al enemigo

        // Volver al pool
        pooled.ReturnToPool();
    }
}
