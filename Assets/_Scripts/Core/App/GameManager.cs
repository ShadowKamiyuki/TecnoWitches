using System;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour, IAppStateMachine
{
    private Dictionary<AppState, IAppState> _states;
    private IAppState _currentState;
    private LoadingRequest _pendingRequest;

    public AppState CurrentState { get; private set; }

    public event Action<AppState> OnStateChanged;

    private void Awake()
    {
        IRunManager runManager = ServiceLocator.Get<IRunManager>();
    }

    private void Start()
    {
        RequestSceneChange(new LoadingRequest(
            load: new[] { "MainMenu" },
            unload: Array.Empty<string>(),
            nextState: AppState.MainMenu
            )
        );
    }

    private void OnDestroy()
    {
        if (ServiceLocator.Exists<IAppStateMachine>())
            ServiceLocator.UnregisterGlobal<IAppStateMachine>();
    }

    public void RegisterStates(Dictionary<AppState, IAppState> states)
    {
        _states = states;
    }

    public void SetState(AppState newState)
    {
        if (CurrentState == newState)
            return;

        _currentState?.Exit();

        if (!_states.TryGetValue(newState, out IAppState nextState))
        {
            Debug.LogError("Estado no encontrado" + newState);
            return;
        }

        CurrentState = newState;
        _currentState = nextState;

        Debug.Log($"[StateMachine] AppState -> {newState}");

        _currentState.Enter();
        OnStateChanged?.Invoke(newState);
    }

    // This saves the request in the manager, then consumes it.
    public void RequestSceneChange(LoadingRequest request)
    {
        _pendingRequest = request;
        SetState(AppState.Loading);
    }

    public LoadingRequest ConsumePendingRequest()
    {
        LoadingRequest request = _pendingRequest;
        _pendingRequest = null;
        return request;
    }
}
