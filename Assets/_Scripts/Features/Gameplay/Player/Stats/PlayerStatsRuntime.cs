public class PlayerStatsRuntime
{
    public Stat MaxHealth { get; }
    public Stat Damage { get; }
    public Stat Speed { get; }
    public Stat MaxEnergy { get; }

    public PlayerStatsRuntime(float maxHealth, float damage, float speed)
    {
        MaxHealth = new Stat(maxHealth);
        Damage = new Stat(damage);
        Speed = new Stat(speed);
    }
}