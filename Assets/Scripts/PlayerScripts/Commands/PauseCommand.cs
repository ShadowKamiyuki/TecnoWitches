public class PauseCommand : ICommand
{
    public void Execute()
    {
        if (GameManager.Instance.currentState.gameState == GameManager.GameState.Gameplay)
        {
            GameManager.Instance.SetGameState(GameManager.GameState.Paused);
        }
        else if (GameManager.Instance.currentState.gameState == GameManager.GameState.Paused)
        {
            GameManager.Instance.SetGameState(GameManager.GameState.Gameplay);
        }
    }
}
