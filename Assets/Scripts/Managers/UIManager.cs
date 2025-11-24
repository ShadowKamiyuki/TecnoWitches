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
    private Coroutine cooldownRoutine;
    public Image healthBar;
    public TextMeshProUGUI healthText;

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

    public void DisplayCooldown(float cooldown)
    {
        if (cooldownRoutine != null)
            StopCoroutine(cooldownRoutine);

        switchCooldown.fillAmount = 1f;
        cooldownRoutine = StartCoroutine(Cooldown(cooldown));
    }

    private IEnumerator Cooldown(float cooldown)
    {
        float timeElapsed = 0;

        while (timeElapsed < cooldown)
        {
            timeElapsed += Time.deltaTime;
            switchCooldown.fillAmount = Mathf.Clamp01(1f - (timeElapsed / cooldown));
            yield return null;
        }

        switchCooldown.fillAmount = 0f;
        cooldownRoutine = null;
    }

    #region OnActionButton

    public void OnResumeGameClicked()
    {
        gameManager.SetGameState(GameManager.GameState.Gameplay);
    }

    public void OnMainMenuButtonClicked()
    {
        DestroySingleton();
        asyncLoader.LoadLevelBtn("Bootstrap Scene");
        gameManager.SetGameState(GameManager.GameState.MainMenu);
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
