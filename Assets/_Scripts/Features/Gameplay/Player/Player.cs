using UnityEngine;

public class Player : MonoBehaviour
{
    private PlayerStatsRuntime _stats;

    private PlayerMovement _movement;
    private PlayerCombat _combat;
    private PlayerInteraction _interaction;

    private PlayerHealth _health;
    private PlayerEnergy _energy;

    private void Awake()
    {
        _movement = GetComponent<PlayerMovement>();
        _combat = GetComponent<PlayerCombat>();
        _interaction = GetComponent<PlayerInteraction>();
        _health = GetComponent<PlayerHealth>();
        _energy = GetComponent<PlayerEnergy>();
    }

    public void Initialize(PlayerCreationData stats)
    {
        _stats = new PlayerStatsRuntime(
           stats.MaxHealth,
           stats.Damage,
           stats.MoveSpeed
        );

        _movement.InjectStats(_stats);
        _combat.InjectStats(_stats);
        _health.Initialize(_stats);
        _energy.Initialize(_stats);
    }

    public void Move(Vector2 direction)
    {
        _movement.SetMoveDirection(direction);
    }

    public void Dodge()
    {
        _movement.TryDodge(0.5f);
    }

    public void Attack()
    {
        _combat.Attack();
    }

    public void SpecialAttack()
    {
        _combat.SpecialAttack();
    }

    public void Interact()
    {
        _interaction.Interact();
    }
}