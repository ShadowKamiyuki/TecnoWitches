using System;
using UnityEngine;

[Serializable]
public abstract class SpellEffect
{
    public abstract void Execute(SpellContext ctx);

    protected GameObject SpawnFromPool(GameObject prefab, Vector3 pos, Quaternion rot)
    {
        return null;
        //return ServiceLocator.Get<PoolManager>().Spawn(prefab, pos, rot);
    }
}