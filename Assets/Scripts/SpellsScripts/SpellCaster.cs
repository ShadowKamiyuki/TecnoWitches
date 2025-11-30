using UnityEngine;

/// <summary>
/// This class handles the cast of the spells without knowing the specific implementations of each spell.
/// We use methods to cast a spell, to change the spell and add new spells.
/// To cast we have the settings of the isometric cursor and a firing point from where the spell origins.
/// </summary>
public class SpellCaster : MonoBehaviour
{
    [Header("Transform settings")]
    [SerializeField] private IsometricCrosshair isoCursor;
    [SerializeField] private Transform firePoint; // from where we shoot

    [Header("Spell List")]
    //[SerializeField] private List<SpellScriptableObject> availableSpells;
    //private SpellScriptableObject currentSpell;
    //[HideInInspector] public SpellScriptableObject GetCurrentSpell() => currentSpell;

    private int currentSpellIndex = 0;

    private void Start()
    {
        // set the current selected spell to the first spell available
        //if (availableSpells.Count > 0)
        //{
        //    currentSpellIndex = 0;
        //    currentSpell = availableSpells[currentSpellIndex];
        //}
    }

    public void CastSpell()
    {
        // we get the position of the cursor to set the direction to shoot to.
        Vector3 direction = (isoCursor.CursorPosition - firePoint.position).normalized;

        //currentSpell.Attack(direction, firePoint);
    }

    public void SwitchSpell(int delta)
    {
        //if (availableSpells.Count == 0) return;

        //currentSpellIndex += delta;
        //if (currentSpellIndex < 0) currentSpellIndex = availableSpells.Count - 1;
        //if (currentSpellIndex >= availableSpells.Count) currentSpellIndex = 0;

        //currentSpell = availableSpells[currentSpellIndex];
        //Debug.Log("Switched to spell: " + currentSpell.GetName());
    }

    public void AddNewSpell(SpellScriptableObject newSpell)
    {
        if (newSpell == null)
        {
            Debug.LogWarning("Trying to add a null spell.");
            return;
        }

        // evitar duplicados
        //if (availableSpells.Contains(newSpell))
        //{
        //    Debug.Log($"Spell '{newSpell.GetName()}' already learned.");
        //    return;
        //}

        //availableSpells.Add(newSpell);

        Debug.Log($"New spell learned: {newSpell.GetName()}");
    }
}
