using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIManager : MonoBehaviourSingleton<UIManager>
{
    private Dictionary<Image, Coroutine> cooldownRoutines = new Dictionary<Image, Coroutine>();

    [Header("Screens")]
    public GameObject pauseScreen;
    public GameObject resultScreen;

    [Header("HUD elements")]
    [SerializeField] private Image switchCooldown;
    [SerializeField] private Image dodgeCooldown;
    [SerializeField] private Image specialCooldown;

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

    public void DisplayCooldown(Image cooldownImage, float cooldown, System.Action onFinish = null)
    {
        // Si ya hay un cooldown activo para esta imagen -> cancelarlo
        if (cooldownRoutines.TryGetValue(cooldownImage, out Coroutine routine) && routine != null)
            StopCoroutine(routine);

        // Reiniciar fillAmount
        cooldownImage.fillAmount = 1f;

        // Crear nueva rutina
        Coroutine newRoutine = StartCoroutine(CooldownRoutine(cooldownImage, cooldown, onFinish));

        // Guardar referencia
        cooldownRoutines[cooldownImage] = newRoutine;
    }

    private IEnumerator CooldownRoutine(Image target, float cooldown, System.Action onFinish)
    {
        float timer = cooldown;

        while (timer > 0f)
        {
            timer -= Time.deltaTime;
            target.fillAmount = timer / cooldown;
            yield return null;
        }

        target.fillAmount = 0f;

        // Ejecutar callback opcional
        onFinish?.Invoke();

        cooldownRoutines[target] = null;
    }

    public void DisplayDodgeCooldown(float cooldown) =>
        DisplayCooldown(dodgeCooldown, cooldown);

    public void DisplaySwitchCooldown(float cooldown) =>
        DisplayCooldown(switchCooldown, cooldown);

    public void DisplaySpecialCooldown(float cooldown) =>
        DisplayCooldown(specialCooldown, cooldown);

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
