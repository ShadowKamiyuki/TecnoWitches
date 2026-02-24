using System.Threading.Tasks;
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

        LoadingRequest request = _stateMachine.ConsumePendingRequest();

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

        // Buscar LoadingView
        LoadingView loadingView = Object.FindFirstObjectByType<LoadingView>();

        if (loadingView != null)
            await loadingView.FadeInAsync();

        // Ejecutar carga sin bloquear
        Task loadingTask = sceneLoader.ProcessRequest(request);

        // Mientras carga -> actualizar progreso
        while (sceneLoader.IsLoading)
        {
            if (loadingView != null)
                loadingView.SetProgress(sceneLoader.Progress);

            await Task.Yield();
        }

        if (loadingView != null)
            await loadingView.FadeOutAsync();

        _stateMachine.SetState(request.NextState);
    }

    public void Exit()
    {
        Debug.Log("Exited Loading State");
    }
}
