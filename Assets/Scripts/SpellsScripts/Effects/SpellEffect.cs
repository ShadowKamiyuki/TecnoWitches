using System;
using System.Collections;
using UnityEngine;

[Serializable]
public abstract class SpellEffect
{
    public abstract void Execute(SpellContext ctx);

    protected GameObject SpawnFromPool(GameObject prefab, Vector3 pos, Quaternion rot)
    {
        return ServiceLocator.Get<PoolManager>().Spawn(prefab, pos, rot);
    }
}