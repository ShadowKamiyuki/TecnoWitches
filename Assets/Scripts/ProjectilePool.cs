using UnityEngine;
using UnityEngine.Pool;

public class ProjectilePool : MonoBehaviour
{
    public GameObject projectilePrefab;
    public int defaultSize = 10;

    private IObjectPool<GameObject> pool;

    public IObjectPool<GameObject> Pool => pool ??= new ObjectPool<GameObject>( CreatePooledObject, OnTakeFromPool, OnReturnedToPool, OnDestroyPoolObject, collectionCheck: false, defaultCapacity: defaultSize, maxSize: 100);

    private GameObject CreatePooledObject()
    {
        GameObject obj = Instantiate(projectilePrefab);
        PooledProjectile pooled = obj.GetComponent<PooledProjectile>();
        pooled.SetPool(Pool);
        obj.SetActive(false);
        return obj;
    }

    private void OnTakeFromPool(GameObject obj)
    {
        obj.SetActive(true);
    }

    private void OnReturnedToPool(GameObject obj)
    {
        obj.SetActive(false);
    }

    private void OnDestroyPoolObject(GameObject obj)
    {
        Destroy(obj);
    }
}
