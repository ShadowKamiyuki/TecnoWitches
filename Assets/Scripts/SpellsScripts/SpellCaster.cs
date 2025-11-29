using UnityEngine;

public class SpellCaster : MonoBehaviour
{
    [SerializeField] private Transform firePoint; // from where we shoot
    [SerializeField] private ISpell[] availableSpells; // Todos los hechizos disponibles
    private int currentSpellIndex = 0;
    private ISpell currentSpell;

    private void Start()
    {
        if (availableSpells.Length > 0)
        {
            currentSpellIndex = 0;
            currentSpell = availableSpells[currentSpellIndex];
        }
    }

    private void Update()
    {
        HandleSpellInput();
        HandleSwitchSpell();
    }

    private void HandleSpellInput()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Vector3 direction = (mousePos - firePoint.position).normalized;
            direction.y = 0;

            currentSpell?.Attack(direction, firePoint);
        }
    }

    private void HandleSwitchSpell()
    {
        if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            SwitchSpell(1);
        }
        else if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            SwitchSpell(-1);
        }
    }

    private void SwitchSpell(int delta)
    {
        if (availableSpells.Length == 0) return;

        currentSpellIndex += delta;
        if (currentSpellIndex < 0) currentSpellIndex = availableSpells.Length - 1;
        if (currentSpellIndex >= availableSpells.Length) currentSpellIndex = 0;

        currentSpell = availableSpells[currentSpellIndex];
        Debug.Log("Switched to spell: " + currentSpell.GetName());
    }
}
