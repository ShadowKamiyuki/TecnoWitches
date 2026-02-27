using System;
using UnityEngine;

public class UnityInputService : IInputService
{
    private PlayerControls _controls;

    public Vector2 Movement => _controls.Gameplay.Move.ReadValue<Vector2>();

    public Vector2 MousePosition => _controls.Gameplay.Point.ReadValue<Vector2>();

    public event Action AttackStarted;
    public event Action AttackCanceled;
    public event Action DodgePressed;
    public event Action InteractPressed;
    public event Action PausePressed;

    public UnityInputService()
    {
        _controls = new PlayerControls();

        _controls.Gameplay.Attack.started += _ => AttackStarted?.Invoke();
        _controls.Gameplay.Attack.canceled += _ => AttackCanceled?.Invoke();
        _controls.Gameplay.Dodge.performed += _ => DodgePressed?.Invoke();
        _controls.Gameplay.Interact.performed += _ => InteractPressed?.Invoke();
        _controls.Gameplay.Pause.performed += _ => PausePressed?.Invoke();

        _controls.Enable();
    }

    public void EnableGameplay()
    {
        _controls.UI.Disable();
        _controls.Gameplay.Enable();
    }

    public void EnableUI()
    {
        _controls.Gameplay.Disable();
        _controls.UI.Enable();
    }
}