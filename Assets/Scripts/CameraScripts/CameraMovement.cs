using UnityEngine;

public class CameraMovement : MonoBehaviour, IUpdatable
{
    [SerializeField] private Transform target;
    [SerializeField] private Vector3 offset;

    private void Awake()
    {
        ServiceLocator.Get<CustomUpdateManager>().Register(this);
    }

    private void OnDestroy()
    {
        CustomUpdateManager updateManager = ServiceLocator.Get<CustomUpdateManager>();

        if (updateManager != null)
        {
            updateManager.Unregister(this);
        }
    }

    public void Tick(float deltaTime)
    {
        transform.position = target.position + offset;
    }
}
