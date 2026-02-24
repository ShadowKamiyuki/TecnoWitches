using UnityEngine;

public class RunController : MonoBehaviour
{
    [SerializeField] private CharacterDatabase characterDatabase;
    [SerializeField] private Transform spawnPoint;

    void Start()
    {
        IRunManager runManager = ServiceLocator.Get<IRunManager>();
        IPlayerFactory playerFactory = ServiceLocator.Get<IPlayerFactory>();

        string characterId = runManager.CurrentCharacterID;

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

        playerFactory.CreatePlayer(creationData, spawnPoint.position);
    }
}
