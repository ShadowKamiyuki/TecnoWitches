using UnityEngine;

public class RoomView : MonoBehaviour
{
    [SerializeField] private GameObject doorNorth;
    [SerializeField] private GameObject doorSouth;
    [SerializeField] private GameObject doorEast;
    [SerializeField] private GameObject doorWest;

    private void Awake()
    {
        doorNorth?.SetActive(false);
        doorSouth?.SetActive(false);
        doorEast?.SetActive(false);
        doorWest?.SetActive(false);
    }

    public void OpenDoor(DoorDirection direction)
    {
        switch (direction)
        {
            case DoorDirection.North: doorNorth?.SetActive(true); break;
            case DoorDirection.South: doorSouth?.SetActive(true); break;
            case DoorDirection.East: doorEast?.SetActive(true); break;
            case DoorDirection.West: doorWest?.SetActive(true); break;
        }
    }
}