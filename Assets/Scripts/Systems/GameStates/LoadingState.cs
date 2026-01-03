using System.Threading.Tasks;
using UnityEngine;

public class LoadingState : BaseState
{
    private SceneLoaderService sceneLoader;
    private LoadingRequest request;

    private FadeController fade;
    private LoadingUIController ui;

    private const string LOADING_UI = "UI_Loading";

    public LoadingState(GameManager gameManager) : base(gameManager)
    {
        sceneLoader = ServiceLocator.Get<SceneLoaderService>();
    }

    public override void Enter()
    {
        base.Enter();
        Time.timeScale = 0f;

        request = gameManager.ConsumeLoadingRequest();
        RunLoadingFlow();
    }

    private async void RunLoadingFlow()
    {
        await sceneLoader.LoadSceneAsync(LOADING_UI);

        await Task.Yield();

        fade = Object.FindFirstObjectByType<FadeController>();
        ui = Object.FindFirstObjectByType<LoadingUIController>();

        if (fade != null)
            await fade.FadeInAsync();

        if (request.scenesToUnload != null)
        {
            foreach (var scene in request.scenesToUnload)
                await sceneLoader.UnloadSceneAsync(scene);
        }

        if (request.scenesToLoad != null)
        {
            foreach (var scene in request.scenesToLoad)
                await sceneLoader.LoadSceneAsync(scene);
        }

        if (fade != null)
            await fade.FadeOutAsync();

        await sceneLoader.UnloadSceneAsync(LOADING_UI);

        Time.timeScale = 1f;
        gameManager.SetGameState(request.NextState);
    }

    public override void Exit()
    {
        base.Exit();
    }
}
