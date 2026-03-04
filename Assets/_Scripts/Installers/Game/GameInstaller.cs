using UnityEngine;

public class GameInstaller : MonoBehaviour
{
    [SerializeField] private DungeonService dungeonService;

    private void Awake()
    {
        if (dungeonService == null)
        {
            Debug.LogError("DungeonService not assigned.");
            return;
        }

        ServiceLocator.RegisterRun<IDungeonService>(dungeonService);
    }

    private void OnDestroy()
    {
        ServiceLocator.ClearRunServices();
    }
}