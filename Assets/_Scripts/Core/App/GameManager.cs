using System;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour, IAppStateMachine
{
    private Dictionary<AppState, IAppState> _states;
    private IAppState _currentState;
    private IInputService _input;
    private LoadingRequest _pendingRequest;

    public AppState CurrentState { get; private set; }

    public event Action<AppState> OnStateChanged;

    private void Awake()
    {
        IRunManager runManager = ServiceLocator.Get<IRunManager>();
        _input = ServiceLocator.Get<IInputService>();
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

    private void OnEnable()
    {
        _input.PausePressed += HandlePause;
    }

    private void OnDestroy()
    {
        _input.PausePressed -= HandlePause;

        if (ServiceLocator.Exists<IAppStateMachine>())
            ServiceLocator.UnregisterGlobal<IAppStateMachine>();
    }

    public void RegisterStates(Dictionary<AppState, IAppState> states)
    {
        _states = states;
    }

    public void SetState(AppState newState, object payload = null)
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

        _currentState.Enter(payload);
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

    // control for pause
    private void HandlePause()
    {
        if (CurrentState == AppState.Gameplay)
            SetState(AppState.Paused);
        else if (CurrentState == AppState.Paused)
            SetState(AppState.Gameplay);
    }
}
