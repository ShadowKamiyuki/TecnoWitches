using UnityEngine;

public class PlayerFactory : IPlayerFactory
{
    public GameObject CreatePlayer(PlayerCreationData data, Vector3 position)
    {
        GameObject playerGO = Object.Instantiate(data.Prefab, position, Quaternion.identity);

        Player player = playerGO.GetComponent<Player>();
        player.Initialize(data);

        return playerGO;
    }
}