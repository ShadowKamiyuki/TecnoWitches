using UnityEngine;

public class Bootstrap : MonoBehaviour
{
    [SerializeField] private GameObject gameManager;
    [SerializeField] private GameObject mainMenu;
    [SerializeField] private GameObject updateManager;

    private void Awake()
    {
        Debug.Log("=== Game Bootstrap iniciado ===");

        Instantiate(updateManager);
        Instantiate(gameManager);
        Instantiate(mainMenu);

        Debug.Log("=== Todos los servicios registrados ===");
    }

    private void Start()
    {
        Debug.Log("cacheando servicios");

        GameManager gm = ServiceLocator.Get<GameManager>();

        if (gm != null)
            gm.Initialize();
    }
}
