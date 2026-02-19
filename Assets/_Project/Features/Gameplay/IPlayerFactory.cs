using UnityEngine;

public interface IPlayerFactory
{
    GameObject SpawnPlayer(string characterId, Transform spawnPoint);
}
