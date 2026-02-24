public class GameplayState : IAppState
{
    private readonly IRunManager _runManager;

    public GameplayState(IRunManager runManager)
    {
        _runManager = runManager;
    }

    public void Enter()
    {
        // La run ya debería estar iniciada
        if (!_runManager.IsRunActive)
        {
            UnityEngine.Debug.LogError("Entered GameplayState without active run.");
        }

        UnityEngine.Time.timeScale = 1f;
    }

    public void Exit() { }
}