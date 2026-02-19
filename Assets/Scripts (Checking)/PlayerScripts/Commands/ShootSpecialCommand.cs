public class ShootSpecialCommand : ICommand
{
    private PlayerActions _player;

    public ShootSpecialCommand(PlayerActions player)
    {
        _player = player;
    }

    public void Execute()
    {
        _player.SpecialAttack();
    }
}
