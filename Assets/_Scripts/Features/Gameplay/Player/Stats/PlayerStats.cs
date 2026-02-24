public sealed class PlayerStats
{
    public float MaxHealth { get; private set; }
    public float Damage { get; private set; }
    public float Speed { get; private set; }

    public PlayerStats(float health, float damage, float speed)
    {
        MaxHealth = health;
        Damage = damage;
        Speed = speed;
    }
}
