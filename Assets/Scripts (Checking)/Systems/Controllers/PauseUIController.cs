using UnityEngine;

public class PauseUIController : MonoBehaviour
{
    private PausedState state;

    public void Initialize(PausedState state)
    {
        this.state = state;
    }

    public void OnResumePressed()
    {
        //state.ResumeGame();
    }

    public void OnMainMenuPressed()
    {
        //state.GoToMainMenu();
    }
}