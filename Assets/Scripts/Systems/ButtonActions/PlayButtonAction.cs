using UnityEngine;

public class PlayButtonAction : MonoBehaviour
{
    public void OnPlayButtonClicked()
    {
        ServiceLocator.Get<GameManager>().SetGameState(GameManager.GameState.Gameplay);
    }
}
