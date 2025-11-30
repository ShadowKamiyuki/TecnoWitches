using UnityEngine;

public class SpellContext
{
    public Vector3 direction;
    public Transform firePoint;
    public GameObject caster;

    public SpellContext(Vector3 direction, Transform firePoint, GameObject caster)
    {
        this.direction = direction;
        this.firePoint = firePoint;
        this.caster = caster;
    }
}
