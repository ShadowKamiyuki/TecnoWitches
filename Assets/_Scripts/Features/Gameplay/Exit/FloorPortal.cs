using UnityEngine;

public class FloorPortal : MonoBehaviour
{
    private IRunManager _runManager;

    private void Awake()
    {
        _runManager = ServiceLocator.Get<IRunManager>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player"))
            return;

        _runManager.AdvanceFloor();
    }
}
