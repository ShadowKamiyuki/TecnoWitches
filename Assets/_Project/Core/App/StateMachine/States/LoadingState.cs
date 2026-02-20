using UnityEngine;

public class LoadingState : IAppState
{
    private readonly IAppStateMachine _stateMachine;

    public LoadingState(IAppStateMachine stateMachine)
    {
        _stateMachine = stateMachine;
    }

    public async void Enter()
    {
        Debug.Log("Entered Loading State");

        GameManager gameManager = _stateMachine as GameManager;
        LoadingRequest request = gameManager?.ConsumePendingRequest();

        if (request == null)
        {
            Debug.LogError("No LoadingRequest found.");
            return;
        }

        ISceneLoader sceneLoader = ServiceLocator.Get<ISceneLoader>();

        if (sceneLoader == null)
        {
            Debug.LogError("ISceneLoader not found.");
            return;
        }

        await sceneLoader.ProcessRequest(request);

        _stateMachine.SetState(request.NextState);
    }

    public void Exit()
    {
        Debug.Log("Exited Loading State");
    }
}
