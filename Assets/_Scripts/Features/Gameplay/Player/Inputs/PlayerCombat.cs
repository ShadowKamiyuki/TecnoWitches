using UnityEngine;

public class PlayerCombat : MonoBehaviour
{
    private SpellCaster _spellCaster;
    private PlayerEnergy _energy;
    private PlayerStatsRuntime _stats;

    private void Awake()
    {
        _spellCaster = GetComponent<SpellCaster>();
        _energy = GetComponent<PlayerEnergy>();
    }

    public void InjectStats(PlayerStatsRuntime stats)
    {
        _stats = stats;
    }

    public void Attack()
    {
        float damage = _stats.Damage.Value;
        //_spellCaster.CastSpell();
    }

    public void SpecialAttack()
    {
        _energy.Use(100f);
    }
}