using UnityEngine;

public class PlayerEnergy : MonoBehaviour, IUpdatable
{
    private PlayerStats playerStats;
    private UIManager uiManager;

    private void Awake()
    {
        ServiceLocator.Get<CustomUpdateManager>().Register(this);
    }

    private void Start()
    {
        uiManager = ServiceLocator.Get<UIManager>();

        playerStats = GetComponent<PlayerStats>();

        uiManager.energyText.text = $"{playerStats.CurrentEnergy} / {playerStats.CharacterData.stats.maxEnergy}";
    }

    private void OnDestroy()
    {
        CustomUpdateManager updateManager = ServiceLocator.Get<CustomUpdateManager>();

        if (updateManager != null)
        {
            updateManager.Unregister(this);
        }
    }

    public void Tick(float deltaTime)
    {
        Recover();
    }

    private void Recover()
    {
        float maxEnergy = playerStats.CharacterData.stats.maxEnergy;
        if (playerStats.CurrentEnergy < maxEnergy)
        {
            playerStats.CurrentEnergy += playerStats.CurrentEnergyRecovery * Time.deltaTime;
            playerStats.CurrentEnergy = Mathf.Min(playerStats.CurrentEnergy, maxEnergy);
            UpdateEnergyBar();
        }
    }

    public void UseEnergy(float amount)
    {
        if (playerStats)
        {
            if (playerStats.CurrentEnergy >= amount)
            {
                playerStats.CurrentEnergy -= amount;
            }
            else
            {
                Debug.Log("not enough energy to cast");
                // here we can add some feedback
            }
        }
    }

    private void UpdateEnergyBar()
    {
        if (uiManager == null || uiManager.energyBar == null)
            return;

        float maxEnergy = playerStats.CharacterData.stats.maxEnergy;
        uiManager.energyBar.fillAmount = playerStats.CurrentEnergy / maxEnergy;
        uiManager.energyText.text = $"{Mathf.RoundToInt(playerStats.CurrentEnergy)} / {playerStats.CharacterData.stats.maxEnergy}";
    }
}
