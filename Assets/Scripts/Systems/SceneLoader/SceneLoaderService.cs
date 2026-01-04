using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoaderService : PersistentService<SceneLoaderService>
{
    public bool IsLoading { get; private set; }
    public float Progress { get; private set; }

    public async Task LoadSceneAsync(string sceneName)
    {
        if (IsLoading)
            return;

        if (SceneManager.GetSceneByName(sceneName).isLoaded)
            return;

        IsLoading = true;
        Progress = 0f;

        AsyncOperation operation = SceneManager.LoadSceneAsync(sceneName, LoadSceneMode.Additive);

        while (!operation.isDone)
        {
            Progress = Mathf.Clamp01(operation.progress / 0.9f);
            await Task.Yield();
        }

        Progress = 1f;
        IsLoading = false;
    }

    public async Task UnloadSceneAsync(string sceneName)
    {
        if (IsLoading)
            return;

        Scene scene = SceneManager.GetSceneByName(sceneName);

        if (!scene.isLoaded)
            return;

        IsLoading = true;
        Progress = 0f;

        AsyncOperation operation =
            SceneManager.UnloadSceneAsync(scene);

        while (!operation.isDone)
        {
            Progress = Mathf.Clamp01(operation.progress / 0.9f);
            await Task.Yield();
        }

        Progress = 1f;
        IsLoading = false;
    }
}
