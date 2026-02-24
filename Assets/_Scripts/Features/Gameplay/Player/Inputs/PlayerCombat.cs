using UnityEngine;

public class PlayerCombat : MonoBehaviour
{
    private SpellCaster _spellCaster;
    private PlayerEnergy _energy;

    private void Awake()
    {
        _spellCaster = GetComponent<SpellCaster>();
        _energy = GetComponent<PlayerEnergy>();
    }

    public void Attack()
    {
        _spellCaster.CastSpell();
    }

    public void SpecialAttack()
    {
        _energy.UseEnergy(100f);
    }
}