using UnityEngine;

public class Player : MonoBehaviour
{
    private PlayerMovement _movement;
    private PlayerCombat _combat;
    private PlayerInteraction _interaction;

    private void Awake()
    {
        _movement = GetComponent<PlayerMovement>();
        _combat = GetComponent<PlayerCombat>();
        _interaction = GetComponent<PlayerInteraction>();
    }

    public void Initialize(PlayerStats stats)
    {

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