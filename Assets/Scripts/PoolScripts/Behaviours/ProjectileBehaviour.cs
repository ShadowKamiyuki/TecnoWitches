using UnityEngine;

[RequireComponent(typeof(Rigidbody), typeof(Collider), typeof(PoolableObject))]
public class ProjectileBehaviour : MonoBehaviour
{
    [SerializeField] private float lifetime = 5f;

    private Rigidbody rb;
    private Collider col;
    private bool isDespawning;
    private PoolableObject poolable;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        col = GetComponent<Collider>();
        poolable = GetComponent<PoolableObject>();
    }

    private void OnEnable()
    {
        isDespawning = false;
        ResetProjectile();
        ScheduleReturnToPool();
    }

    private void OnDisable()
    {
        CancelInvoke();
    }

    public void Launch(Vector3 direction, float speed)
    {
        rb.velocity = direction.normalized * speed;
    }

    public void ResetProjectile()
    {
        // Resetea las propiedades del proyectil a su estado inicial
        // Rigidbody
        rb.velocity = Vector3.zero;

        col.enabled = true;
    }

    private void ScheduleReturnToPool()
    {
        // Cancela cualquier invocación previa y programa la devolución al pool
        CancelInvoke();
        Invoke(nameof(ReturnToPool), lifetime);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Enemy"))
        {
            ReturnToPool();
        }
    }

    private void ReturnToPool()
    {
        if (isDespawning)
            return;

        isDespawning = true;

        CancelInvoke();
        col.enabled = false;

        poolable.Despawn();
    }
}
