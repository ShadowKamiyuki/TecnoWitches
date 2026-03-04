using UnityEngine;

public class GameplayState : IAppState
{
    private readonly IRunManager _runManager;

    public GameplayState(IRunManager runManager)
    {
        _runManager = runManager;
    }

    public void Enter(object payload)
    {
        Time.timeScale = 1f;

        Debug.Log($"GameplayState payload type: {payload?.GetType()}");

        // Si viene desde CharacterSelect -> iniciar run
        if (payload is RunStartRequest request)
        {
            Debug.Log($"GameplayState received ID: '{request.CharacterID}'");
            _runManager.StartRun(request.CharacterID);
            return;
        }

        // Si no hay payload, simplemente estamos volviendo (ej: desde Pause)
        if (!_runManager.IsRunActive)
        {
            Debug.LogError("GameplayState entered without active run.");
        }
    }

    public void Exit() { }
}