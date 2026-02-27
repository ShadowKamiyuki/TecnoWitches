using System;
using UnityEngine;

public interface IInputService
{
    Vector2 Movement { get; }
    Vector2 MousePosition { get; }

    event Action AttackStarted;
    event Action AttackCanceled;
    event Action DodgePressed;
    event Action InteractPressed;
    event Action PausePressed;

    void EnableGameplay();
    void EnableUI();
}