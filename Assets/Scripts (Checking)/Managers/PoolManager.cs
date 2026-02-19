using System.Collections.Generic;
using UnityEngine;

public class PoolManager : MonoBehaviour
{
    // Diccionario donde cada prefab tiene su propia cola de objetos disponibles.
    // key = prefab original
    // value = cola de instancias desactivadas listas para reutilizar
    private Dictionary<GameObject, Queue<GameObject>> pools = new();
    private Dictionary<GameObject, Transform> poolParents = new();

    [SerializeField] private Transform poolsRoot; // opcional

    private void Awake()
    {
        ServiceLocator.RegisterGlobal<PoolManager>(this);
    }

    private void OnDestroy()
    {
        ServiceLocator.UnregisterGlobal<PoolManager>();
    }

    /// <summary>
    /// Pre-instancia X objetos del prefab para evitar picos de lag al inicio.
    /// </summary>
    public void Prewarm(GameObject prefab, int amount)
    {
        if (!pools.ContainsKey(prefab))
            pools[prefab] = new Queue<GameObject>();

        Transform parent = GetPoolParent(prefab);

        for (int i = 0; i < amount; i++)
        {
            GameObject obj = Instantiate(prefab, parent);
            obj.SetActive(false);

            // save reference to original prefab
            var po = obj.AddComponent<PoolableObject>();
            po.OriginalPrefab = prefab;

            pools[prefab].Enqueue(obj);
        }
    }

    /// <summary>
    /// Obtiene un objeto del pool o crea uno nuevo si no hay disponibles.
    /// </summary>
    public GameObject Spawn(GameObject prefab, Vector3 pos, Quaternion rot)
    {
        if (!pools.ContainsKey(prefab))
            pools[prefab] = new Queue<GameObject>();

        GameObject obj;

        if (pools[prefab].Count > 0)
        {
            obj = pools[prefab].Dequeue();
            obj.transform.SetParent(null); // libre en escena
            obj.transform.SetPositionAndRotation(pos, rot);
            obj.SetActive(true);
        }
        else
        {
            obj = Instantiate(prefab, pos, rot);

            var po = obj.GetComponent<PoolableObject>() ?? obj.AddComponent<PoolableObject>();
            po.OriginalPrefab = prefab;
        }

        return obj;
    }

    /// <summary>
    /// Devuelve una instancia al pool.
    /// </summary>
    public void Despawn(GameObject instance)
    {
        var po = instance.GetComponent<PoolableObject>();
        if (po == null || po.OriginalPrefab == null)
        {
            Debug.LogError("Intentando despawnear un objeto no poolable");
            return;
        }

        if (!pools.ContainsKey(po.OriginalPrefab))
            pools[po.OriginalPrefab] = new Queue<GameObject>();

        Transform parent = GetPoolParent(po.OriginalPrefab);

        instance.SetActive(false);
        instance.transform.SetParent(parent);

        pools[po.OriginalPrefab].Enqueue(instance);
    }

    private Transform GetPoolParent(GameObject prefab)
    {
        if (poolParents.TryGetValue(prefab, out Transform parent))
            return parent;

        GameObject go = new GameObject(prefab.name + "_Pool");
        parent = go.transform;

        if (poolsRoot != null)
            parent.SetParent(poolsRoot);

        poolParents[prefab] = parent;
        return parent;
    }
}
