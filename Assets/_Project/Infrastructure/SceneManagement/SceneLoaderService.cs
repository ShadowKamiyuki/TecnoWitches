using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoaderService : MonoBehaviour, ISceneLoader
{
    public bool IsLoading { get; private set; }
    public float Progress { get; private set; }

    private const string LOADING_SCENE_NAME = "Loading";
    private const float MIN_LOADING_TIME = 0.6f;

    public async Task ProcessRequest(LoadingRequest request)
    {
        if (IsLoading)
            return;

        IsLoading = true;
        Progress = 0f;

        float startTime = Time.unscaledTime;

        // 1. Cargar Loading scene
        AsyncOperation loadingScreenOp = SceneManager.LoadSceneAsync(LOADING_SCENE_NAME, LoadSceneMode.Additive);
        await AwaitOperation(loadingScreenOp, null);

        // Esperar 1 frame para que Awake se ejecute
        await Task.Yield();

        // Obtener FadeController desde Loading
        LoadingView loadingView = Object.FindFirstObjectByType<LoadingView>();

        if (loadingView != null)
            await loadingView.FadeInAsync();

        // 2. Descargar escenas anteriores
        foreach (var scene in request.ScenesToUnload)
        {
            AsyncOperation unloadOp = SceneManager.UnloadSceneAsync(scene);
            await AwaitOperation(unloadOp, loadingView);
        }

        // 3. Cargar escenas nuevas
        foreach (var scene in request.ScenesToLoad)
        {
            AsyncOperation loadOp = SceneManager.LoadSceneAsync(scene, LoadSceneMode.Additive);
            await AwaitOperation(loadOp, loadingView);
        }

        Progress = 1f;

        // 4. Tiempo mínimo para evitar flash
        float elapsed = Time.unscaledTime - startTime;
        if (elapsed < MIN_LOADING_TIME)
        {
            await Task.Delay(
                Mathf.CeilToInt((MIN_LOADING_TIME - elapsed) * 1000f)
            );
        }

        // 5. Fade out
        if (loadingView != null)
            await loadingView.FadeOutAsync();

        // 6. Descargar Loading
        AsyncOperation unloadingScreenOp = SceneManager.UnloadSceneAsync(LOADING_SCENE_NAME);
        await AwaitOperation(unloadingScreenOp, null);

        IsLoading = false;

        // 7. Cambiar estado
        ServiceLocator.Get<IAppStateMachine>().SetState(request.NextState);
    }

    private async Task AwaitOperation(AsyncOperation operation, LoadingView loadingView)
    {
        while (!operation.isDone)
        {
            Progress = Mathf.Clamp01(operation.progress / 0.9f);

            if (loadingView != null)
                loadingView.SetProgress(Progress);

            await Task.Yield();
        }
    }
}
