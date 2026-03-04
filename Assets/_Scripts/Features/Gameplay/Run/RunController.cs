using UnityEngine;

public class RunController : MonoBehaviour
{
    [SerializeField] private CharacterDatabase characterDatabase;
    [SerializeField] private Transform spawnPoint;
    [SerializeField] private CameraMovement playerCamera;

    [SerializeField] private GameObject player;

    private IRunManager _runManager;

    private void Awake()
    {
        _runManager = ServiceLocator.Get<IRunManager>();
        _runManager.OnRunStarted += HandleRunStarted;
    }

    private void OnDestroy()
    {
        if (_runManager != null)
        {
            _runManager.OnRunStarted -= HandleRunStarted;
            _runManager.OnRunEnded -= HandleRunEnded;
        }
    }


    void HandleRunStarted()
    {
        IPlayerFactory playerFactory = ServiceLocator.Get<IPlayerFactory>();

        string characterId = _runManager.CurrentCharacterID;

        CharacterData character = characterDatabase.GetById(characterId);

        if (character == null)
        {
            Debug.LogError($"Character with ID {characterId} not found!");
            return;
        }

        PlayerCreationData creationData = new PlayerCreationData(
            character.CharacterPrefab,
            character.BaseStats.maxHealth,
            character.BaseStats.damage,
            character.BaseStats.moveSpeed
        );

        player = playerFactory.CreatePlayer(creationData, spawnPoint.position);

        playerCamera.SetCameraTarget(player.transform);
        playerCamera.gameObject.SetActive(true);
    }

    private void HandleRunEnded()
    {
        if (player != null)
            Destroy(player);

        playerCamera.gameObject.SetActive(false);
    }

    public void MovePlayerTo(Vector3 position)
    {
        if (player != null)
            player.transform.position = position;
    }
}
