using UnityEngine;

/// <summary>
/// Class used to pass data to the executed spell like position of game objects.
/// This is a data container used to centralize parameters for all the spell logic.
/// </summary>
public class SpellContext
{
    public Vector3 direction { get; }
    public Transform firePoint { get; }
    public GameObject caster {  get; }
    public GameObject target { get; }

    public SpellContext(Vector3 direction, Transform firePoint, GameObject caster, GameObject target = null)
    {
        this.direction = direction;
        this.firePoint = firePoint;
        this.caster = caster;
        this.target = target;
    }
}
