using UnityEngine;

public class PlayerInputController : MonoBehaviour
{
    private PlayerControls _controls;
    private Player _player;

    private void Awake()
    {
        _controls = new PlayerControls();
        _player = GetComponent<Player>();
    }

    private void OnEnable()
    {
        _controls.Enable();

        _controls.Player.Attack.performed += _ => _player.Attack();
        _controls.Player.Dodge.performed += _ => _player.Dodge();
        _controls.Player.Interact.performed += _ => _player.Interact();
    }

    private void OnDisable()
    {
        _controls.Disable();
    }

    private void Update()
    {
        Vector2 move = _controls.Player.Move.ReadValue<Vector2>();
        _player.Move(move);
    }
}
