using UnityEngine;

public class PoolableObject : MonoBehaviour
{
    public GameObject OriginalPrefab { get; set; }
    private bool isDespawned;

    /// <summary>
    /// Llama esto desde tus scripts para devolver el objeto al pool.
    /// </summary>
    public void Despawn()
    {
        if (isDespawned)
            return;

        isDespawned = true;
        ServiceLocator.Get<PoolManager>().Despawn(gameObject);
    }

    private void OnEnable()
    {
        isDespawned = false;
    }
}
