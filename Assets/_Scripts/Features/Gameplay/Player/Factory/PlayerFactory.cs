using UnityEngine;

public class PlayerFactory : IPlayerFactory
{
    public GameObject CreatePlayer(PlayerCreationData data, Vector3 position)
    {
        GameObject playerGO = Object.Instantiate(data.Prefab, position, Quaternion.identity);

        PlayerStats stats = new PlayerStats(
            data.MaxHealth,
            data.Damage,
            data.MoveSpeed
        );

        Player player = playerGO.GetComponent<Player>();
        player.Initialize(stats);

        return playerGO;
    }
}