using UnityEngine;

public abstract class SpellStrategy : ScriptableObject, ISpellStrategy
{
    public abstract void Cast(SpellContext context);
}
