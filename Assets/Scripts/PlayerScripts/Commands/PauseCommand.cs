public class PauseCommand : ICommand
{
    public void Execute()
    {
        GameManager gm = ServiceLocator.Get<GameManager>();

        if (gm.currentState.gameState == GameManager.GameState.Gameplay)
        {
            gm.SetGameState(GameManager.GameState.Paused);
        }
        else if (gm.currentState.gameState == GameManager.GameState.Paused)
        {
            gm.SetGameState(GameManager.GameState.Gameplay);
        }
    }
}
