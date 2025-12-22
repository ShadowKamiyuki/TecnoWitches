using UnityEngine;

public class HealthPickup : Pickup
{
    [SerializeField] private int healthAmount = 25;

    protected override void Collect(GameObject collector)
    {
        collector.GetComponent<PlayerHealth>()?.RestoreHealth(healthAmount);
    }
}
