using System.Collections.Generic;
using UnityEngine;

public class SpellCaster : MonoBehaviour
{
    [Header("Transform settings")]
    [SerializeField] private IsometricCrosshair isoCursor;
    [SerializeField] private Transform firePoint; // from where we shoot
    [SerializeField] private float firingHeight;

    [Header("Spell List")]
    [SerializeField] private List<ISpell> availableSpells; // Todos los hechizos disponibles

    private int currentSpellIndex = 0;
    private ISpell currentSpell;

    private void Start()
    {
        if (availableSpells.Count > 0)
        {
            currentSpellIndex = 0;
            currentSpell = availableSpells[currentSpellIndex];
        }
    }

    public void HandleSpell()
    {
        Vector3 direction = (isoCursor.CursorPosition - firePoint.position).normalized;
        direction.y = firingHeight;

        currentSpell.Attack(direction, firePoint);
    }

    public void SwitchSpell(int delta)
    {
        if (availableSpells.Count == 0) return;

        currentSpellIndex += delta;
        if (currentSpellIndex < 0) currentSpellIndex = availableSpells.Count - 1;
        if (currentSpellIndex >= availableSpells.Count) currentSpellIndex = 0;

        currentSpell = availableSpells[currentSpellIndex];
        Debug.Log("Switched to spell: " + currentSpell.GetName());
    }

    public void AddNewSpell(ISpell newSpell)
    {
        if (newSpell == null)
        {
            Debug.LogWarning("Trying to add a null spell.");
            return;
        }

        // evitar duplicados
        if (availableSpells.Contains(newSpell))
        {
            Debug.Log($"Spell '{newSpell.GetName()}' already learned.");
            return;
        }

        availableSpells.Add(newSpell);

        Debug.Log($"New spell learned: {newSpell.GetName()}");
    }
}
