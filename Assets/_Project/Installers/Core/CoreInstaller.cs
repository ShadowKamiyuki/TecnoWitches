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

        ServiceLocator.RegisterGlobal<IRunManager>(new RunManager());
        ServiceLocator.RegisterGlobal<ISaveService>(new SaveService());

        Debug.Log("=== Servicios globales registrados ===");
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
