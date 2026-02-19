using UnityEngine;

public class PlayerFactory : IPlayerFactory
{
    private readonly GameObject _warriorPrefab;
    private readonly GameObject _magePrefab;
    private readonly GameObject _roguePrefab;

    public PlayerFactory(
        GameObject warrior,
        GameObject mage,
        GameObject rogue)
    {
        _warriorPrefab = warrior;
        _magePrefab = mage;
        _roguePrefab = rogue;
    }

    public GameObject SpawnPlayer(string characterId, Transform spawnPoint)
    {
        GameObject prefab = characterId switch
        {
            "warrior" => _warriorPrefab,
            "mage" => _magePrefab,
            "rogue" => _roguePrefab,
            _ => _warriorPrefab
        };

        return Object.Instantiate(prefab, spawnPoint.position, spawnPoint.rotation);
    }
}
