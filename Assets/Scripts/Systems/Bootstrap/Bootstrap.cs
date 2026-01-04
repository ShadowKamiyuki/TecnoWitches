using UnityEngine;
using UnityEngine.EventSystems;

public class Bootstrap : MonoBehaviour
{
    [Header("Core Services")]
    [SerializeField] private GameObject gameManager;
    [SerializeField] private GameObject updateManager;
    [SerializeField] private GameObject audioManager;
    [SerializeField] private GameObject sceneLoader;
    [SerializeField] private GameObject levelController;
    [SerializeField] private GameObject eventSystemPrefab;

    private void Awake()
    {
        Debug.Log("=== Game Bootstrap iniciado ===");

        InstantiateIfNeeded<CustomUpdateManager>(updateManager);
        InstantiateIfNeeded<GameManager>(gameManager);
        InstantiateIfNeeded<AudioManager>(audioManager);
        InstantiateIfNeeded<SceneLoaderService>(sceneLoader);
        InstantiateIfNeeded<LevelController>(levelController);
        InstantiateIfNeeded<EventSystem>(eventSystemPrefab);

        Debug.Log("=== Managers creados ===");
    }

    private void Start()
    {
        Debug.Log("cacheando servicios");

        GameManager gm = ServiceLocator.Get<GameManager>();
        gm?.Initialize();
    }

    private void InstantiateIfNeeded<T>(GameObject prefab)
    {
        if (prefab == null)
            return;

        if (ServiceLocator.Exists<T>())
            return;

        Instantiate(prefab);
    }
}
