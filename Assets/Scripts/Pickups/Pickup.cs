using UnityEngine;

public class Pickup : MonoBehaviour, ICollectible, IUpdatable
{
    public bool hasBeenCollected = false;
    private Transform player;

    private void OnEnable()
    {
        ServiceLocator.Get<CustomUpdateManager>().Register(this);
    }

    private void OnDisable()
    {
        CustomUpdateManager updateManager = ServiceLocator.Get<CustomUpdateManager>();

        if (updateManager != null)
        {
            updateManager.Unregister(this);
        }
    }

    public virtual void Collect()
    {
        if (hasBeenCollected) return;

        hasBeenCollected = true;

        Debug.Log("picked up");

        // Por defecto destruimos la pickup
        Destroy(gameObject);
    }

    public void Tick(float deltaTime)
    {
        if (player != null)
        {
            if (Vector2.Distance(transform.position, player.position) < 0.1f)
            {
                Collect();
            }
        }
    }
}
