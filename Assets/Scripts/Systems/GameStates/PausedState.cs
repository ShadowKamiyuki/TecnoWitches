using UnityEngine;

public class PausedState : BaseState
{
    private const string PAUSE_UI_SCENE = "UI_Pause";
    private SceneLoaderService sceneLoader;
    private PauseUIController uiController;

    public PausedState(GameManager gameManager) : base(gameManager)
    {
        sceneLoader = ServiceLocator.Get<SceneLoaderService>();
    }

    public override void Enter()
    {
        base.Enter();
        Time.timeScale = 0f;

        LoadPauseUI();
    }

    public override void Exit()
    {
        Time.timeScale = 1f;
        base.Exit();
    }

    private async void LoadPauseUI()
    {
        await sceneLoader.LoadSceneAsync(PAUSE_UI_SCENE);

        uiController = Object.FindFirstObjectByType<PauseUIController>();
        uiController?.Initialize(this);
    }

    public void ResumeGame()
    {
        LoadingRequest request = new LoadingRequest(
            load: null,
            unload: new[] { PAUSE_UI_SCENE },
            nextState: GameManager.GameState.Gameplay
        );

        gameManager.LoadWithTransition(request);
    }

    public void GoToMainMenu()
    {
        LoadingRequest request = new LoadingRequest(
            load: new[] { "UI_MainMenu" },
            unload: new[] { "Game", PAUSE_UI_SCENE },
            nextState: GameManager.GameState.MainMenu
        );

        gameManager.LoadWithTransition(request);
    }
}
