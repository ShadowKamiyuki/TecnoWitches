using UnityEngine;

public class GameplayState : IAppState
{
    private readonly IRunManager _runManager;
    private readonly IAppStateMachine _stateMachine;

    public GameplayState()
    {
        _runManager = ServiceLocator.Get<IRunManager>();
        _stateMachine = ServiceLocator.Get<IAppStateMachine>();
    }

    public void Enter()
    {
        //if (!_runManager.IsRunActive)
        //    _runManager.StartRun();

        Time.timeScale = 1f;
    }

    public void Exit()
    {
        // Solo terminar la run si NO vamos a pausa
        if (_stateMachine.CurrentState == AppState.GameOver)
        {
            _runManager.EndRun();
        }
    }
}
