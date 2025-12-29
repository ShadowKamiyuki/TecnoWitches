using UnityEngine;

public class DirectionalSpellContext : PositionSpellContext
{
    public Vector3 Direction { get; }

    public DirectionalSpellContext(GameObject caster, Vector3 targetPoint, Vector3 direction, GameObject target = null) : base(caster, targetPoint, target)
    {
        Direction = direction.normalized;
    }
}
