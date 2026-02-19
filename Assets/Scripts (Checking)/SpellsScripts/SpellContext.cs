using UnityEngine;

/// <summary>
/// Class used to pass data to the executed spell like position of game objects.
/// This is a data container used to centralize parameters for all the spell logic.
/// </summary>
public class SpellContext
{
    public GameObject Caster { get; }
    public GameObject Target { get; }

    public SpellContext(GameObject caster, GameObject target = null)
    {
        Caster = caster;
        Target = target;
    }
}
