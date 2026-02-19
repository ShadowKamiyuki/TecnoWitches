using UnityEngine;

public class DoorsController : MonoBehaviour, IUpdatable
{
    [SerializeField] private GameObject[] doors;

    private CustomUpdateManager customUpdate;
    private bool isInsideRoom;
    private bool isRoomCleared;

    private void OnEnable()
    {
        ServiceLocator.Get<CustomUpdateManager>().Register(this);
    }

    private void OnDestroy()
    {
        customUpdate = ServiceLocator.Get<CustomUpdateManager>();
        if (customUpdate != null)
        {
            customUpdate.Unregister(this);
        }
    }

    public void Tick(float deltaTime)
    {
        if (isRoomCleared)
        {
            OpenDoors();
        }
    }

    private void CloseDoors()
    {
        foreach (GameObject door in doors)
        {
            door.transform.position = new Vector3(door.transform.position.x, door.transform.position.y + 5, door.transform.position.z);
        }
    }

    private void OpenDoors()
    {
        foreach (GameObject door in doors)
        {
            door.transform.position = new Vector3(door.transform.position.x, door.transform.position.y - 5, door.transform.position.z);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            CloseDoors();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            OpenDoors();
        }
    }
}
