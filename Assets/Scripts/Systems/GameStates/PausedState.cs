using UnityEngine;

public class PausedState : IState
{
    [HideInInspector] public string Name { get => "MainMenu State"; }
    public GameManager.GameState gameState { get => GameManager.GameState.Paused; }

    private GameManager gameManager;

    public PausedState(GameManager gm)
    {
        gameManager = gm;
    }
    public void Enter()
    {
        Time.timeScale = 0f;
        ServiceLocator.Get<UIManager>().pauseScreen.SetActive(true);
    }

    public void Exit()
    {
        Time.timeScale = 1f;
        ServiceLocator.Get<UIManager>().pauseScreen.SetActive(false);
    }

    public void Update()
    {
        
    }
}
