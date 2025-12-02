using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// This class handles the cast of the spells without knowing the specific implementations of each spell.
/// We use methods to cast a spell, to change the spell and add new spells.
/// To cast we have the settings of the isometric cursor and a firing point from where the spell origins.
/// </summary>
public class SpellCaster : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private IsometricCrosshair isoCursor;
    [SerializeField] private Transform firePoint; // from where we shoot

    [Header("Factory")]
    [SerializeField] private SpellFactory spellFactory;

    [Header("Spell List")]
    [SerializeField] private List<Spell> availableSpells;

    private Spell currentSpell;
    private int currentSpellIndex = 0;

    private void Start()
    {
        // set the current selected spell to the first spell available
        if (availableSpells.Count > 0)
        {
            currentSpellIndex = 0;
            currentSpell = availableSpells[currentSpellIndex];
        }
    }

    public void CastSpell()
    {
        // we get the position of the cursor to set the direction to shoot to.
        Vector3 direction = (isoCursor.CursorPosition - firePoint.position).normalized;

        currentSpell.Cast(new SpellContext(direction, firePoint, gameObject));
    }

    public void SwitchSpell(int delta)
    {
        if (availableSpells.Count == 0) return;

        currentSpellIndex = (currentSpellIndex + delta + availableSpells.Count) % availableSpells.Count;
        currentSpell = availableSpells[currentSpellIndex];

        Debug.Log("Switched to: " + currentSpell.id);
    }

    public void AddNewSpell(Spell newSpell)
    {
        if (newSpell == null)
        {
            Debug.LogWarning("Trying to add a null spell.");
            return;
        }

        // evitar duplicados
        if (availableSpells.Contains(newSpell))
        {
            Debug.Log("Spell already learned.");
            return;
        }

        availableSpells.Add(newSpell);
    }

    public void EquipSpell(Spell spell)
    {
        currentSpell = spell;
    }
}
