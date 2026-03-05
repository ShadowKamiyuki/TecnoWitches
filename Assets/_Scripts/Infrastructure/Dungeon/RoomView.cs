using UnityEngine;
using System.Collections.Generic;

public class RoomView : MonoBehaviour
{
    [Header("Door Anchors (Sockets)")]
    [SerializeField] private Transform northSocket;
    [SerializeField] private Transform southSocket;
    [SerializeField] private Transform eastSocket;
    [SerializeField] private Transform westSocket;

    [Header("Door Visuals")]
    [SerializeField] private GameObject northDoor;
    [SerializeField] private GameObject southDoor;
    [SerializeField] private GameObject eastDoor;
    [SerializeField] private GameObject westDoor;

    private Dictionary<DoorDirection, Transform> _sockets;
    private Dictionary<DoorDirection, GameObject> _doors;

    private void Awake()
    {
        _sockets = new Dictionary<DoorDirection, Transform>
        {
            { DoorDirection.North, northSocket },
            { DoorDirection.South, southSocket },
            { DoorDirection.East, eastSocket },
            { DoorDirection.West, westSocket }
        };

        _doors = new Dictionary<DoorDirection, GameObject>
        {
            { DoorDirection.North, northDoor },
            { DoorDirection.South, southDoor },
            { DoorDirection.East, eastDoor },
            { DoorDirection.West, westDoor }
        };

        // Todas las puertas empiezan cerradas (o desactivadas)
        foreach (var door in _doors.Values)
            if (door != null)
                door.SetActive(false);
    }

    public void OpenDoor(DoorDirection direction)
    {
        if (_doors.TryGetValue(direction, out var door) && door != null)
            door.SetActive(true);
    }

    public Vector3 GetDoorPosition(DoorDirection direction)
    {
        if (_sockets.TryGetValue(direction, out var socket) && socket != null)
            return socket.position;

        return transform.position;
    }
}