using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private IInputService _input;
    private PlayerMovement _movement;
    private PlayerCombat _combat;
    private PlayerInteraction _interaction;

    private void Awake()
    {
        _input = ServiceLocator.Get<IInputService>();
        _movement = GetComponent<PlayerMovement>();
        _combat = GetComponent<PlayerCombat>();
        _interaction = GetComponent<PlayerInteraction>();
    }

    private void OnEnable()
    {
        _input.AttackStarted += OnAttackStarted;
        _input.AttackCanceled += OnAttackCanceled;
        _input.DodgePressed += OnDodge;
        _input.InteractPressed += OnInteract;
    }

    private void OnDisable()
    {
        _input.AttackStarted -= OnAttackStarted;
        _input.AttackCanceled -= OnAttackCanceled;
        _input.DodgePressed -= OnDodge;
        _input.InteractPressed -= OnInteract;
    }

    private void Update()
    {
        _movement.SetMoveDirection(_input.Movement);
    }

    private void OnAttackStarted() => _combat.Attack();
    private void OnAttackCanceled() { }
    private void OnDodge() => _movement.TryDodge(0.5f);
    private void OnInteract() => _interaction.Interact();
}