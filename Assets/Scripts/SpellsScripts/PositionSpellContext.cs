using UnityEngine;

public class PositionSpellContext : SpellContext
{
    public Vector3 TargetPoint { get; }

    public PositionSpellContext(GameObject caster, Vector3 targetPoint, GameObject target = null) : base(caster, target)
    {
        TargetPoint = targetPoint;
    }
}
