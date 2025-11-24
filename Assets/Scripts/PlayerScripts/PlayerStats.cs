using UnityEngine;

public class PlayerStats : MonoBehaviour
{
    private CharacterData characterData;
    public CharacterData CharacterData => characterData;

    public CharacterData.Stats baseStats;
    [SerializeField] private CharacterData.Stats actualStats;

    // current stats
    private float currentHealth;
    private float currentEnergy;
    private float currentEnergyRecovery;
    private float currentMoveSpeed;
    private float currentMight;
    private float currentProjectileSpeed;
    private float currentMagnet;
    private float luck;

    #region Current Stats Properties
    public float CurrentHealth
    {
        get { return currentHealth; }
        set { currentHealth = value; }
    }

    public float CurrentEnergy
    {
        get { return currentEnergy; }
        set { currentEnergy = value; }
    }

    public float CurrentEnergyRecovery
    {
        get { return currentEnergyRecovery; }
        set { currentEnergyRecovery = value; }
    }

    public float CurrentMoveSpeed
    {
        get { return currentMoveSpeed; }
        set { currentMoveSpeed = value; }
    }

    public float CurrentMight
    {
        get { return currentMight; }
        set { currentMight = value; }
    }

    public float CurrentProjectileSpeed
    {
        get { return currentProjectileSpeed; }
        set { currentProjectileSpeed = value; }
    }

    public float CurrentMagnet
    {
        get { return currentMagnet; }
        set { currentMagnet = value; }
    }

    public float Luck { get { return luck; } }

    #endregion

    private void Start()
    {
        characterData = CharacterSelector.GetData();

        if (characterData == null)
        {
            Debug.LogError("Character data not found!");
            return;
        }

        // assign the variables
        baseStats = actualStats = characterData.stats;

        CharacterSelector.instance.DestroySingleton();

        // assign the variables
        CurrentHealth = characterData.stats.maxHealth;
        CurrentEnergy = characterData.stats.maxEnergy;
        CurrentEnergyRecovery = characterData.stats.energyRecovery;
        CurrentMoveSpeed = characterData.stats.moveSpeed;
        CurrentMight = characterData.stats.might;
        CurrentProjectileSpeed = characterData.stats.projectileSpeed;
        CurrentMagnet = characterData.stats.magnet;
    }
}
