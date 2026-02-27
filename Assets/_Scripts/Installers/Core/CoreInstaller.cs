using System.Collections.Generic;
using UnityEngine;

public class CoreInstaller : MonoBehaviour
{
    [Header("Global Services")]
    [SerializeField] private AudioManager audioManager;
    [SerializeField] private CustomUpdateManager updateManager;
    [SerializeField] private GameManager gameManager;
    [SerializeField] private SceneLoaderService sceneLoaderService;

    private void Awake()
    {
        Debug.Log("=== CoreInstaller iniciado ===");

        RegisterService<IAudioService>(audioManager);
        RegisterService<IUpdateService>(updateManager);
        RegisterService<IAppStateMachine>(gameManager);
        RegisterService<ISceneLoader>(sceneLoaderService);

        ISeedGenerator seedGenerator = new UnitySeedGenerator();
        IRunManager runManager = new RunManager(seedGenerator);
        ISaveService saveService = new SaveService();
        IInputService inputService = new UnityInputService();
        IPlayerFactory playerFactory = new PlayerFactory();

        ServiceLocator.RegisterGlobal(runManager);
        ServiceLocator.RegisterGlobal(saveService);
        ServiceLocator.RegisterGlobal(inputService);
        ServiceLocator.RegisterGlobal(playerFactory);

        Debug.Log("=== Servicios globales registrados ===");

        Dictionary<AppState, IAppState> states = new()
        {
            { AppState.MainMenu, new MainMenuState() },
            { AppState.SaveSelect, new SaveSelectState() },
            { AppState.CharacterSelect, new CharacterSelectState() },
            { AppState.Loading, new LoadingState(gameManager) },
            { AppState.Gameplay, new GameplayState(runManager) },
            { AppState.GameOver, new GameOverState() }
        };

        gameManager.RegisterStates(states);

        Debug.Log("=== Estados creados ===");
    }

    private void RegisterService<T>(MonoBehaviour service)
    {
        if (service == null)
        {
            Debug.LogError($"Servicio {typeof(T).Name} es null");
            return;
        }

        if (!service.TryGetComponent(out T typedService))
        {
            Debug.LogError($"{service.name} no implementa {typeof(T).Name}");
            return;
        }

        ServiceLocator.RegisterGlobal<T>(typedService);
    }
}
