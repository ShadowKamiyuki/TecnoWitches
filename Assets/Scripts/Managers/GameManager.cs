using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviourSingleton<GameManager>, IUpdatable
{
    private List<IState> states = new List<IState>();

    public enum GameState
    {
        MainMenu,
        Gameplay,
        Paused,
        GameOver
    }

    // Store the current state of the game
    public IState currentState;

    // Flag to check if the game is over
    public bool isGameOver = false;

    protected override void OnAwaken()
    {
        Debug.Log("GameManager inicializado");

        // Resgistrar en el service locator
        ServiceLocator.Register<GameManager>(this);

        // Registrar en el custom update manager
        ServiceLocator.Get<CustomUpdateManager>().Register(this);

        // Crear y registrar los estados
        states.Add(new MainMenuState(this));
        states.Add(new GameplayState(this));
        states.Add(new PausedState(this));
        states.Add(new GameOverState(this));
    }

    protected override void OnDestroyed()
    {
        CustomUpdateManager updateManager = ServiceLocator.Get<CustomUpdateManager>();

        if (updateManager != null)
        {
            updateManager.Unregister(this);
        }

        ServiceLocator.Unregister<GameManager>();

        Debug.Log("GameManager destruido");
    }

    public void Initialize()
    {
        SetGameState(GameState.MainMenu); // Establecer el estado inicial
    }

    public void Tick(float deltaTime)
    {
        // Delegamos el comportamiento del estado actual
        currentState?.Update();
    }

    // Define the method to change the state of the game
    public void SetGameState(GameState newState)
    {
        // Si hay un estado anterior, llamamos al Exit
        currentState?.Exit();

        // Establecemos el nuevo estado y llamamos al Enter
        // En States BUSCA el state, EN DONDE el state.gameState sea igual a newState    
        currentState = states.Find(state => state.gameState == newState);

        if (currentState == null)
        {
            Debug.LogError("Estado no encontrado: " + newState);
            return;
        }

        // Llamamos al método Enter del nuevo estado
        currentState.Enter();
    }
}
