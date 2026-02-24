using UnityEngine;

public struct PlayerCreationData
{
    public GameObject Prefab;
    public float MaxHealth;
    public float Damage;
    public float MoveSpeed;

    public PlayerCreationData(GameObject prefab, float maxHealth, float damage, float moveSpeed)
    {
        Prefab = prefab;
        MaxHealth = maxHealth;
        Damage = damage;
        MoveSpeed = moveSpeed;
    }
}