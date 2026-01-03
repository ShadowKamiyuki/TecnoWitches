public class PauseCommand : ICommand
{
    public void Execute()
    {
        GameManager gm = ServiceLocator.Get<GameManager>();
        IState currentState = gm.CurrentState;

        if (currentState is GameplayState)
        {
            gm.SetGameState(GameManager.GameState.Paused);
        }
        else if (currentState is PausedState)
        {
            gm.SetGameState(GameManager.GameState.Gameplay);
        }
    }
}
