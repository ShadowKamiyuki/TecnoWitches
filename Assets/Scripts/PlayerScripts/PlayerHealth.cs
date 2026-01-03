using UnityEngine;

public class PlayerHealth : MonoBehaviour, IUpdatable
{
    private PlayerStats playerStats;
    private GameManager gm;
    private UIManager uiManager;

    [Header("Damage particle effect")]
    //[SerializeField] private ParticleSystem damageEffect;

    // I-Frames
    [Header("I-Frames")]
    [SerializeField] private float invincibilityDuration = 0.5f;
    private float invincibilityTimer;
    private bool isInvincible;

    private void Awake()
    {
        ServiceLocator.Get<CustomUpdateManager>().Register(this);
    }

    private void Start()
    {
        gm = ServiceLocator.Get<GameManager>();
        uiManager = ServiceLocator.Get<UIManager>();

        playerStats = GetComponent<PlayerStats>();

        uiManager.healthText.text = $"{playerStats.CurrentHealth} / {playerStats.CharacterData.stats.maxHealth}";
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
        BecomeInvincible();
    }

    private void BecomeInvincible()
    {
        if (invincibilityTimer > 0)
        {
            invincibilityTimer -= Time.deltaTime;
        }
        else if (isInvincible)
        {
            isInvincible = false;
        }
    }

    public void ActivateInvincibility(float duration)
    {
        isInvincible = true;
        invincibilityTimer = duration;
    }

    public void TakeDamage(float dmg)
    {
        if (isInvincible)
            return;

        if (playerStats)
            playerStats.CurrentHealth -= dmg;

        // add the call to particle effect (not instantiate -> pool)
        invincibilityTimer = invincibilityDuration;
        isInvincible = true;

        if (playerStats.CurrentHealth <= 0)
        {
            playerStats.CurrentHealth = 0;
            Kill();
        }

        UpdateHealthBar();
    }

    public void RestoreHealth(float amount)
    {
        float maxHealth = playerStats.CharacterData.stats.maxHealth;
        playerStats.CurrentHealth = Mathf.Min(playerStats.CurrentHealth + amount, maxHealth);
        UpdateHealthBar();
    }

    private void UpdateHealthBar()
    {
        if (uiManager == null || uiManager.healthBar == null)
            return;

        float maxHealth = playerStats.CharacterData.stats.maxHealth;
        uiManager.healthBar.fillAmount = playerStats.CurrentHealth / maxHealth;
        uiManager.healthText.text = $"{playerStats.CurrentHealth} / {playerStats.CharacterData.stats.maxHealth}";
    }

    private void Kill()
    {
        //if (!gm.isGameOver)
        //{
        //    gm.SetGameState(GameManager.GameState.GameOver);
        //}
    }
}
