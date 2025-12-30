using UnityEngine;

public class Bootstrap : MonoBehaviour
{
    [SerializeField] private GameObject gameManager;
    [SerializeField] private GameObject updateManager;
    [SerializeField] private GameObject audioManager;
    [SerializeField] private GameObject sceneLoader;
    [SerializeField] private string initialScene = "UIMenu";

    private void Awake()
    {
        Debug.Log("=== Game Bootstrap iniciado ===");

        Instantiate(updateManager);
        Instantiate(gameManager);
        Instantiate(audioManager);
        Instantiate(sceneLoader);

        Debug.Log("=== Todos los servicios registrados ===");
    }

    private void Start()
    {
        Debug.Log("cacheando servicios");

        GameManager gm = ServiceLocator.Get<GameManager>();

        if (gm != null)
            gm.Initialize();

        ASyncLoader loader = ServiceLocator.Get<ASyncLoader>();
        if (loader != null)
        {
            loader.LoadLevel(initialScene);
        }
    }
}
