using UnityEngine;

public class SwitchCommand : ICommand
{
    private PlayerActions _player;
    private float cooldown;
    private float lastExecutionTime;

    public SwitchCommand(PlayerActions player, float cooldown)
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
            Debug.Log("Cambio dimensional en cooldown...");
            return;
        }

        if (_player.CurrentRoom != null)
        {
            _player.SwitchDimension();
            lastExecutionTime = Time.time;
            UIManager.Instance.DisplayCooldown(cooldown);
            Debug.Log("Dimensión cambiada con éxito!");
        }
    }
}
