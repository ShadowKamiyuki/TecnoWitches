using System;
using UnityEngine;

[Serializable]
public class PersistentAreaEffect : SpellEffect
{
    public GameObject areaPrefab;
    public float duration = 5f;

    public override void Execute(SpellContext ctx)
    {
        GameObject area = GameObject.Instantiate(areaPrefab, ctx.Target.transform.position, Quaternion.identity);
        GameObject.Destroy(area, duration);
    }
}