using UnityEngine;

public class MoveCommand : ICommand
{
    private PlayerActions _playerActions;
    private InputHandler _input;

    public MoveCommand(PlayerActions playerActions, InputHandler inputHandler)
    {
        _playerActions = playerActions;
        _input = inputHandler;
    }

    public void Execute()
    {
        _playerActions.SetMoveDirection(_input.moveDir);
    }
}
