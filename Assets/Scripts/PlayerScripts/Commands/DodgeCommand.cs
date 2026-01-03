using UnityEngine;

public class DodgeCommand : ICommand
{
    private PlayerActions _player;
    private float cooldown;

    public DodgeCommand(PlayerActions player, float cooldown)
    {
        _player = player;
        this.cooldown = cooldown;
    }

    public void Execute()
    {
        if (_player.TryDodge(cooldown))
        {
            //UIManager.Instance.DisplayDodgeCooldown(cooldown);
        }
        else
        {
            Debug.Log("Dodge in cooldown...");
        }
    }
}
