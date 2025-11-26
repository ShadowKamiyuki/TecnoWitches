using UnityEngine;

public class DodgeCommand : ICommand
{
    private PlayerActions _player;
    private float cooldown;
    private float lastExecutionTime;

    public DodgeCommand(PlayerActions player, float cooldown)
    {
        _player = player;
        this.cooldown = cooldown;
        lastExecutionTime = -cooldown;
    }

    public bool CanExecute()
    {
        return Time.time >= lastExecutionTime + cooldown;
    }

    public void Execute()
    {
        if (!CanExecute())
        {
            Debug.Log("Dodge in cooldown...");
            return;
        }

        _player.Dodge(cooldown);
        lastExecutionTime = Time.time;
        UIManager.Instance.DisplayDodgeCooldown(cooldown);
    }
}
