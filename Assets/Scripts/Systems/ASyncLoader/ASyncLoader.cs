using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ASyncLoader : MonoBehaviour
{
    private string currentLevelScene;
    private bool isLoading;

    public bool IsLoading => isLoading;

    public event Action<float> OnProgress;
    public event Action<bool> OnLoadingStateChanged;
    public event Action OnFadeOutRequested;
    public event Action OnFadeInRequested;

    private void OnEnable()
    {
        ServiceLocator.Register(this);
    }

    private void OnDisable()
    {
        ServiceLocator.Unregister<ASyncLoader>();
    }

    public void LoadLevel(string levelToLoad)
    {
        if (isLoading)
            return;

        // Run the ASync
        StartCoroutine(LoadLevelASync(levelToLoad));
    }

    IEnumerator LoadLevelASync(string levelToLoad)
    {
        isLoading = true;
        OnLoadingStateChanged?.Invoke(true);
        OnFadeOutRequested?.Invoke();
        OnProgress?.Invoke(0f);

        yield return null;

        // unload scene
        if (!string.IsNullOrEmpty(currentLevelScene))
        {
            AsyncOperation unloadOperation = SceneManager.UnloadSceneAsync(currentLevelScene);

            while (!unloadOperation.isDone)
            {
                yield return null;
            }
        }

        // load scene
        AsyncOperation loadOperation = SceneManager.LoadSceneAsync(levelToLoad, LoadSceneMode.Additive);

        loadOperation.allowSceneActivation = false;

        while (loadOperation.progress < 0.9f)
        {
            float progress = Mathf.Clamp01(loadOperation.progress / 0.9f);
            OnProgress?.Invoke(progress);
            yield return null;
        }

        OnProgress?.Invoke(1f);
        loadOperation.allowSceneActivation = true;

        while (!loadOperation.isDone)
        {
            yield return null;
        }

        currentLevelScene = levelToLoad;

        Scene loadedScene = SceneManager.GetSceneByName(levelToLoad);
        SceneManager.SetActiveScene(loadedScene);

        OnLoadingStateChanged?.Invoke(true);
        OnFadeInRequested?.Invoke();
        isLoading = false;
    }
}
