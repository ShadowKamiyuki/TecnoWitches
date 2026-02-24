using UnityEngine;

public interface IPlayerFactory
{
    GameObject CreatePlayer(PlayerCreationData data, Vector3 position);
}