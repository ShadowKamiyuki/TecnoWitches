using System;
using UnityEngine;

public abstract class Pickup : MonoBehaviour, IPickUpEffect
{
    protected bool hasBeenCollected = false;

    public event Action OnCollected;

    public void Apply(GameObject collector)
    {
        if (hasBeenCollected)
            return;

        hasBeenCollected = true;
        OnCollected.Invoke();
        Collect(collector);

        // sustitucion por pool a futuro
        Destroy(gameObject);
    }

    protected abstract void Collect(GameObject collector);

    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player")) return;
        Apply(other.gameObject);
    }
}
