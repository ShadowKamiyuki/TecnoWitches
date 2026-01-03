using System;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : PersistentService<GameManager>, IUpdatable
{
    public enum GameState
    {
        MainMenu,
        Gameplay,
        Paused,
        GameOver,
        Loading,
        CharacterSelect
    }

    private Dictionary<GameState, IState> _states;
    private LoadingRequest pendingRequest;

    // Store the current state of the game
    public IState CurrentState { get; private set; }

    public event Action<GameState> OnGameStateChanged;

    protected override void OnAwakeService()
    {
        Debug.Log("GameManager inicializado");

        // Registrar en el custom update manager
        ServiceLocator.Get<CustomUpdateManager>()?.Register(this);
    }

    protected override void OnDestroyService()
    {
        CustomUpdateManager updateManager = ServiceLocator.Get<CustomUpdateManager>();

        if (updateManager != null)
            updateManager.Unregister(this);

        Debug.Log("GameManager destruido");
    }

    public void Initialize()
    {
        CreateStates();
        SetGameState(GameState.MainMenu); // Establecer el estado inicial
    }

    public void Tick(float deltaTime)
    {
        // Delegamos el comportamiento del estado actual
        CurrentState?.Update(deltaTime);
    }

    // Define the method to change the state of the game
    public void SetGameState(GameState newState)
    {
        if (CurrentState == _states[newState])
            return;

        // Si hay un estado anterior, llamamos al Exit
        CurrentState?.Exit();

        if (!_states.TryGetValue(newState, out IState nextState))
        {
            Debug.LogError("Estado no encontrado: " + newState);
            return;
        }

        CurrentState = nextState;
        // Llamamos al método Enter del nuevo estado
        CurrentState.Enter();
        OnGameStateChanged?.Invoke(newState);
    }

    public void CreateStates()
    {
        if (_states != null)
            return;

        _states = new Dictionary<GameState, IState>()
        {
            { GameState.MainMenu, new MainMenuState(this) },
            { GameState.Gameplay, new GameplayState(this) },
            { GameState.Paused, new PausedState(this) },
            { GameState.GameOver, new GameOverState(this) },
            { GameState.Loading, new LoadingState(this) },
            { GameState.CharacterSelect, new CharacterSelectState(this) }
        };
    }

    public void LoadWithTransition(LoadingRequest request)
    {
        pendingRequest = request;
        SetGameState(GameState.Loading);
    }

    public LoadingRequest ConsumeLoadingRequest()
    {
        LoadingRequest request = pendingRequest;
        pendingRequest = null;
        return request;
    }
}
