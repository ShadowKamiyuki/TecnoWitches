using UnityEngine;

public class CoinPickup : Pickup
{
    [SerializeField] private int value = 1;

    protected override void Collect(GameObject collector)
    {
        // agregar logica de monedas aqui
        Debug.Log($"picked up a coin, value = {value}");
    }
}
