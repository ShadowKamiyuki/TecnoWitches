using UnityEngine;

public interface ISpell
{
    public void Cast(SpellContext context);
    public string GetName();
}
