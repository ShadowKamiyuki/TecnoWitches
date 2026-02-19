using System;
using System.Threading.Tasks;
using UnityEngine;

public class LevelController : MonoBehaviour
{
    public event Action OnLevelLoaded;
    public string CurrentLevelScene { get; private set; }

    private SceneLoaderService sceneLoader => ServiceLocator.Get<SceneLoaderService>();

    private string loadedLevel;

    // Cargar un nivel aditivo
    public async Task LoadLevel(string levelName)
    {
        // Si hay un nivel cargado, se descarga
        if (!string.IsNullOrEmpty(loadedLevel))
        {
            //await sceneLoader.UnloadSceneAsync(loadedLevel);
        }

        loadedLevel = levelName;
        CurrentLevelScene = levelName;

        // Cargar el nuevo nivel
        //await sceneLoader.LoadSceneAsync(levelName);

        // Notificar que el nivel se cargó
        OnLevelLoaded?.Invoke();
    }
}
