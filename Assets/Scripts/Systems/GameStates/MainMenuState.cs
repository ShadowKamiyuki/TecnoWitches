using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UIElements;

public class MainMenuState : BaseState
{
    private SceneLoaderService sceneLoader;
    private MainMenuStateController menuController; // referencia al controller de UI

    private const string MAIN_MENU_SCENE = "UI_MainMenu";

    public MainMenuState(GameManager gameManager) : base(gameManager)
    {
        sceneLoader = ServiceLocator.Get<SceneLoaderService>();
    }

    public override void Enter()
    {
        base.Enter();
        Time.timeScale = 0f; // Pausar el tiempo mientras estamos en el menú

        LoadMenuUI();
    }

    public override void Exit()
    {
        if (menuController != null)
        {
            // Fade out antes de cambiar de estado
            gameManager.StartCoroutine(menuController.FadeOut(0.5f));
            ServiceLocator.Unregister<MainMenuStateController>();
        }

        Time.timeScale = 1f;
        base.Exit();
    }

    public void GoToCharacterSelect()
    {
        LoadingRequest request = new LoadingRequest(load: new[] { "UI_CharacterSelect" }, unload: new[] { "UI_MainMenu" }, nextState: GameManager.GameState.CharacterSelect);

        gameManager.LoadWithTransition(request);
    }

    private async void LoadMenuUI()
    {
        await sceneLoader.LoadSceneAsync(MAIN_MENU_SCENE);
        await Task.Yield(); // Espera un frame para asegurarse de que la escena esté activa

        menuController = Object.FindFirstObjectByType<MainMenuStateController>();
        if (menuController != null)
        {
            menuController.Initialize(this);
            ServiceLocator.Register(menuController);

            // iniciar fade in de la pantalla
            gameManager.StartCoroutine(menuController.FadeIn(0.5f));
        }
    }
}
