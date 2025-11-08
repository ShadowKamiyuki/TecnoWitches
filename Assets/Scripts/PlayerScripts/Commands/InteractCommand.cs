public class InteractCommand : ICommand
{
    private PlayerActions _player;

    public InteractCommand(PlayerActions player)
    {
        _player = player;
    }

    public void Execute()
    {
        _player.Interact();
    }
}
