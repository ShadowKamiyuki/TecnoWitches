public class DodgeCommand : ICommand
{
    private PlayerActions _player;

    public DodgeCommand(PlayerActions player)
    {
        _player = player;
    }

    public void Execute()
    {
        _player.Dodge();
    }
}
