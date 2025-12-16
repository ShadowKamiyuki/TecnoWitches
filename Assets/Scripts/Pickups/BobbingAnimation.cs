using UnityEngine;

public class BobbingAnimation : MonoBehaviour, IUpdatable
{
    public float frecuency; // speed of movement
    public float magnitude; // range of movement
    public Vector3 direction; // direction of movement

    private Vector3 initialPosition;
    private Pickup pickup;

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

    private void Start()
    {
        pickup = GetComponent<Pickup>();

        // save the starting position of the game object
        initialPosition = transform.position;
    }

    public void Tick(float deltaTime)
    {
        if (pickup && !pickup.hasBeenCollected)
        {
            // sin function for smooth bobbing effect
            transform.position = initialPosition + direction * Mathf.Sin(Time.time * frecuency) * magnitude;
        }
    }
}
