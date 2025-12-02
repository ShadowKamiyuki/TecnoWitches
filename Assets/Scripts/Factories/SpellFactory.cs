using UnityEngine;
using System.Collections.Generic;

[CreateAssetMenu(fileName = "SpellFactory", menuName = "TecnoWitches/Spell Factory")]
public class SpellFactory : ScriptableObject
{
    [SerializeField] private List<Spell> allSpells;

    private Dictionary<string, Spell> lookup;

    private void OnEnable()
    {
        lookup = new Dictionary<string, Spell>();

        foreach (var spell in allSpells)
        {
            lookup[spell.id] = spell;
        }
    }

    public Spell Get(string id)
    {
        if (lookup.TryGetValue(id, out var spell))
            return spell;

        Debug.LogError("Spell not found: " + id);
        return null;
    }

    public void Cast(string id, SpellContext ctx)
    {
        Get(id)?.Cast(ctx);
    }
}
