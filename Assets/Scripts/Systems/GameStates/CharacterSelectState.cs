using System.Threading.Tasks;
using UnityEngine;

public class CharacterSelectState : BaseState
{
    private SceneLoaderService sceneLoader;
    private SelectionMenuUIController menuController; // referencia al controller de la pantalla

    private CharacterData currentSelectedCharacter;

    private const string CHARACTER_SELECT_SCENE = "UI_CharacterSelect";

    public CharacterSelectState(GameManager gameManager) : base(gameManager)
    {
        sceneLoader = ServiceLocator.Get<SceneLoaderService>();
    }

    public override void Enter()
    {
        base.Enter();
        Time.timeScale = 0f;

        LoadSelectionMenu();
    }

    public override void Exit()
    {
        if (menuController != null)
        {
            gameManager.StartCoroutine(menuController.FadeOut(0.5f));
        }

        Time.timeScale = 1f;
        base.Exit();
    }

    // llamado desde el UIController cuando el jugador hace click en un personaje
    public void SetSelectedCharacter(CharacterData character)
    {
        currentSelectedCharacter = character;
    }

    // El jugador confirma personaje
    public void ConfirmSelection()
    {
        CharacterSelectionService service = ServiceLocator.Get<CharacterSelectionService>();
        service.SelectCharacter(currentSelectedCharacter); // tu variable interna de selección

        LoadingRequest request = new LoadingRequest(
            load: new[] { "Game" },
            unload: new[] { "UI_CharacterSelect" },
            nextState: GameManager.GameState.Gameplay
        );

        gameManager.LoadWithTransition(request);
    }

    public bool HasSelection()
    {
        return currentSelectedCharacter != null;
    }

    // El jugador vuelve al menú principal
    public void BackToMainMenu()
    {
        LoadingRequest request = new LoadingRequest(
            load: new[] { "UI_MainMenu" },
            unload: new[] { "UI_CharacterSelect" },
            nextState: GameManager.GameState.MainMenu
        );

        gameManager.LoadWithTransition(request);
    }

    private async void LoadSelectionMenu()
    {
        await sceneLoader.LoadSceneAsync(CHARACTER_SELECT_SCENE);
        await Task.Yield();

        menuController = Object.FindFirstObjectByType<SelectionMenuUIController>();
        if (menuController != null)
        {
            menuController.Initialize(this);

            // iniciar fade in de la pantalla
            gameManager.StartCoroutine(menuController.FadeIn(0.5f));
        }
    }
}
