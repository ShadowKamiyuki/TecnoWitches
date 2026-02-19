using UnityEngine;

public class LoadingState : IAppState
{
    private readonly GameManager _gameManager;
    private readonly IAppStateMachine _stateMachine;
    private readonly ISceneLoader _sceneLoader;

    public LoadingState(GameManager gm)
    {
        _gameManager = gm;
        _sceneLoader = ServiceLocator.Get<ISceneLoader>();
        _stateMachine = ServiceLocator.Get<IAppStateMachine>();
    }

    public async void Enter()
    {
        LoadingRequest request = _gameManager.ConsumePendingRequest();

        if (request == null)
        {
            Debug.LogError("No hay LoadingRequest pendiente.");
            return;
        }

        await _sceneLoader.ProcessRequest(request);

        // Cambiar estado después de completar carga
        _stateMachine.SetState(request.NextState);
    }

    public void Exit()
    {
        Debug.Log("Saliendo de Loading");
        // Ocultar pantalla de carga
    }
}