public class NextCommand : ICommand
{
    private PlayerActions _player;

    public NextCommand(PlayerActions player)
    {
        _player = player;
    }

    public void Execute()
    {
        _player.NextSpell();
    }
}
