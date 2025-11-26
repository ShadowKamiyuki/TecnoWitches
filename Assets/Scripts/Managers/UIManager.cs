using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIManager : MonoBehaviourSingleton<UIManager>
{
    [Header("Screens")]
    public GameObject pauseScreen;
    public GameObject resultScreen;

    [Header("HUD elements")]
    [SerializeField] private Image switchCooldown;
    private Coroutine SwitchCooldownRoutine;
    [SerializeField] private Image dodgeCooldown;
    private Coroutine dodgeCooldownRoutine;

    public Image healthBar;
    public TextMeshProUGUI healthText;
    public Image energyBar;
    public TextMeshProUGUI energyText;

    [Header("ASync Loader")]
    [SerializeField] private ASyncLoader asyncLoader;

    private GameManager gameManager;

    protected override void OnAwaken()
    {
        ServiceLocator.Register<UIManager>(this);
        gameManager = ServiceLocator.Get<GameManager>();
        DisableScreens();
    }

    private void OnDestroy()
    {
        ServiceLocator.Unregister<UIManager>();
    }

    public void DisableScreens()
    {
        pauseScreen.SetActive(false);
        resultScreen.SetActive(false);
    }

    public void DestroySingleton()
    {
        Destroy(UIManager.Instance.gameObject);
    }

    public void DisplaySwitchCooldown(float cooldown)
    {
        if (SwitchCooldownRoutine != null)
            StopCoroutine(SwitchCooldownRoutine);

        switchCooldown.fillAmount = 1f;
        SwitchCooldownRoutine = StartCoroutine(SwitchCooldown(cooldown));
    }

    private IEnumerator SwitchCooldown(float cooldown)
    {
        float timeElapsed = 0;

        while (timeElapsed < cooldown)
        {
            timeElapsed += Time.deltaTime;
            switchCooldown.fillAmount = Mathf.Clamp01(1f - (timeElapsed / cooldown));
            yield return null;
        }

        switchCooldown.fillAmount = 0f;
        SwitchCooldownRoutine = null;
    }

    public void DisplayDodgeCooldown(float cooldown)
    {
        if (dodgeCooldownRoutine != null)
            StopCoroutine(dodgeCooldownRoutine);

        dodgeCooldown.fillAmount = 1f;
        dodgeCooldownRoutine = StartCoroutine(DodgeCooldown(cooldown));
    }

    private IEnumerator DodgeCooldown(float cooldown)
    {
        float timeElapsed = 0;

        while (timeElapsed < cooldown)
        {
            timeElapsed += Time.deltaTime;
            dodgeCooldown.fillAmount = Mathf.Clamp01(1f - (timeElapsed / cooldown));
            yield return null;
        }

        dodgeCooldown.fillAmount = 0f;
        dodgeCooldownRoutine = null;
    }

    #region OnActionButton

    public void OnResumeGameClicked()
    {
        gameManager.SetGameState(GameManager.GameState.Gameplay);
    }

    public void OnMainMenuButtonClicked()
    {
        asyncLoader.LoadLevelBtn("Bootstrap Scene");
        gameManager.SetGameState(GameManager.GameState.MainMenu);
        DestroySingleton();
    }

    public void OnRestartButtonClicked()
    {
        DestroySingleton();
        asyncLoader.LoadLevelBtn("GameScene");
        gameManager.SetGameState(GameManager.GameState.Gameplay);
        resultScreen.SetActive(false);
    }

    #endregion
}
