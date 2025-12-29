using UnityEngine;

public class FirePointSpellContext : DirectionalSpellContext
{
    public Transform FirePoint { get; }

    public FirePointSpellContext(GameObject caster, Transform firePoint, Vector3 targetPoint, Vector3 direction, GameObject target = null) : base(caster, targetPoint, direction, target)
    {
        FirePoint = firePoint;
    }
}
