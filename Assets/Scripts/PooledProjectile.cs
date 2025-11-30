using UnityEngine;
using UnityEngine.Pool;

public class PooledProjectile : MonoBehaviour
{
    private IObjectPool<GameObject> pool;
    private bool isReleased = false;
    private Rigidbody rb;

    [SerializeField] private float lifeTime = 3f;
    private float lifeTimer;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }

    private void Update()
    {
        if (!isReleased)
        {
            lifeTimer -= Time.deltaTime;
            if (lifeTimer <= 0)
                ReturnToPool();
        }
    }

    public void SetPool(IObjectPool<GameObject> pool)
    {
        this.pool = pool;
    }

    public void ReturnToPool()
    {
        if (isReleased) return;
        isReleased = true;

        if (rb != null)
        {
            rb.velocity = Vector3.zero;
            rb.angularVelocity = Vector3.zero;
        }

        pool.Release(gameObject);
    }

    private void OnCollisionEnter(Collision collision)
    {
        ReturnToPool();
    }

    private void OnEnable()
    {
        isReleased = false;
        lifeTimer = lifeTime;
    }
}
