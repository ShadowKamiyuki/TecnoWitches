using System.Collections.Generic;
using UnityEngine;

public class PoolManager : MonoBehaviourSingleton<PoolManager>
{
    // Diccionario donde cada prefab tiene su propia cola de objetos disponibles.
    // key = prefab original
    // value = cola de instancias desactivadas listas para reutilizar
    private Dictionary<GameObject, Queue<GameObject>> pools = new();

    protected override void OnAwaken()
    {
        ServiceLocator.Register<PoolManager>(this);
    }

    private void OnDestroy()
    {
        ServiceLocator.Unregister<PoolManager>();
    }

    /// <summary>
    /// Pre-instancia X objetos del prefab para evitar picos de lag al inicio.
    /// </summary>
    public void Prewarm(GameObject prefab, int amount)
    {
        if (!pools.ContainsKey(prefab))
            pools[prefab] = new Queue<GameObject>();

        for (int i = 0; i < amount; i++)
        {
            GameObject obj = Instantiate(prefab);
            obj.SetActive(false);

            // Guardamos referencia al prefab original (importante!)
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
            obj.transform.SetPositionAndRotation(pos, rot);
            obj.SetActive(true);
        }
        else
        {
            obj = Instantiate(prefab, pos, rot);

            var po = obj.GetComponent<PoolableObject>();
            if (po == null)
                po = obj.AddComponent<PoolableObject>();

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

        instance.transform.SetParent(null);
        instance.SetActive(false);

        pools[po.OriginalPrefab].Enqueue(instance);
    }
}
