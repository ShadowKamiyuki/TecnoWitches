public class PreviousCommand : ICommand
{
    private PlayerActions _player;

    public PreviousCommand(PlayerActions player)
    {
        _player = player;
    }

    public void Execute()
    {
        _player.PreviousSpell();
    }
}
