using UnityEngine;

public class GameOverUIController : MonoBehaviour
{
    private GameOverState state;

    public void Initialize(GameOverState state)
    {
        this.state = state;
    }

    // Botón Retry
    public void OnRetryPressed()
    {
        //state?.Retry();
    }

    // Botón Main Menu
    public void OnMainMenuPressed()
    {
        //state?.GoToMainMenu();
    }
}
