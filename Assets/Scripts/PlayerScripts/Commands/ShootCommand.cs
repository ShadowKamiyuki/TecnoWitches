public class ShootCommand : ICommand
{
    private PlayerActions _player;

    public ShootCommand(PlayerActions player)
    {
        _player = player;
    }

    public void Execute()
    {
        _player.Attack();
    }
}
