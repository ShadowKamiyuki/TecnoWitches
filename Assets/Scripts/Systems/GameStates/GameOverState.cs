using UnityEngine;

public class GameOverState : BaseState
{
    private const string GAME_OVER_UI = "UI_GameOver";
    private SceneLoaderService sceneLoader;
    private GameOverUIController uiController;

    public GameOverState(GameManager gameManager) : base(gameManager)
    {
        sceneLoader = ServiceLocator.Get<SceneLoaderService>();
    }

    public override void Enter()
    {
        base.Enter();
        Time.timeScale = 0f;

        LoadGameOverUI();
    }

    public override void Exit()
    {
        Time.timeScale = 1f;
        base.Exit();
    }

    private async void LoadGameOverUI()
    {
        await sceneLoader.LoadSceneAsync(GAME_OVER_UI);

        uiController = Object.FindFirstObjectByType<GameOverUIController>();
        uiController?.Initialize(this);
    }

    public void Retry()
    {
        LoadingRequest request = new LoadingRequest(
            load: new[] { "Game" },
            unload: new[] { GAME_OVER_UI },
            nextState: GameManager.GameState.Gameplay
        );

        gameManager.LoadWithTransition(request);
    }

    public void GoToMainMenu()
    {
        LoadingRequest request = new LoadingRequest(
            load: new[] { "UI_MainMenu" },
            unload: new[] { "Game", GAME_OVER_UI },
            nextState: GameManager.GameState.MainMenu
        );

        gameManager.LoadWithTransition(request);
    }
}
