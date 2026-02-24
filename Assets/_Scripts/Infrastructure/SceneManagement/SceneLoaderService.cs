using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoaderService : MonoBehaviour, ISceneLoader
{
    public bool IsLoading { get; private set; }
    public float Progress { get; private set; }

    //private const string LOADING_SCENE_NAME = "Loading";
    private const float MIN_LOADING_TIME = 0.6f;

    public async Task ProcessRequest(LoadingRequest request)
    {
        if (IsLoading)
            return;

        IsLoading = true;
        Progress = 0f;

        float startTime = Time.unscaledTime;

        // Descargar escenas anteriores
        foreach (var scene in request.ScenesToUnload)
        {
            AsyncOperation unloadOp = SceneManager.UnloadSceneAsync(scene);
            await AwaitOperation(unloadOp);
        }

        // Cargar escenas nuevas
        foreach (var scene in request.ScenesToLoad)
        {
            AsyncOperation loadOp = SceneManager.LoadSceneAsync(scene, LoadSceneMode.Additive);
            await AwaitOperation(loadOp);
        }

        Progress = 1f;

        // Garantizar tiempo minimo
        float elapsed = Time.unscaledTime - startTime;

        if (elapsed < MIN_LOADING_TIME)
        {
            await Task.Delay(Mathf.CeilToInt((MIN_LOADING_TIME - elapsed) * 1000f));
        }

        IsLoading = false;
    }

    private async Task AwaitOperation(AsyncOperation operation)
    {
        while (!operation.isDone)
        {
            Progress = Mathf.Clamp01(operation.progress / 0.9f);
            await Task.Yield();
        }
    }
}
