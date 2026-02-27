using UnityEngine;
using UnityEngine.SceneManagement;

public class RunFlowController : MonoBehaviour
{
    private IRunManager _runManager;

    private void Awake()
    {
        _runManager = ServiceLocator.Get<IRunManager>();
        _runManager.OnRunStarted += LoadLobby;
        _runManager.OnFloorChanged += LoadDungeon;
    }

    private void LoadLobby()
    {
        SceneManager.LoadScene("Lobby", LoadSceneMode.Additive);
    }

    private void LoadDungeon(int floor)
    {
        SceneManager.LoadScene("DungeonScene", LoadSceneMode.Additive);
    }
}