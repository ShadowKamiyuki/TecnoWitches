using UnityEngine;

public class RunFlowController : MonoBehaviour
{
    private IRunManager _runManager;
    private IDungeonService _dungeonService;

    [SerializeField] private DungeonGenerationConfig config;
    [SerializeField] private TransitionService transitionService;
    [SerializeField] private RunController runController;

    private void Awake()
    {
        _runManager = ServiceLocator.Get<IRunManager>();

        _runManager.OnRunStarted += HandleRunStarted;
        _runManager.OnFloorChanged += HandleFloorChanged;
    }

#if UNITY_EDITOR
    [SerializeField] private bool generateOnStart = true;
#endif

    private void Start()
    {
        _dungeonService = ServiceLocator.Get<IDungeonService>();

#if UNITY_EDITOR
        if (generateOnStart)
        {
            _dungeonService.Generate(config);
            runController.MovePlayerTo(_dungeonService.PlayerSpawn);
        }
#endif
    }

    private void HandleRunStarted()
    {
        _dungeonService.Generate(config);
        runController.MovePlayerTo(_dungeonService.PlayerSpawn);
    }

    private async void HandleFloorChanged(int floor)
    {
        await transitionService.FadeIn();

        _dungeonService.Generate(config);
        runController.MovePlayerTo(_dungeonService.PlayerSpawn);

        await transitionService.FadeOut();
    }
}