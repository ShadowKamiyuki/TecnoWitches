using System;
using UnityEngine;

public class DimensionalSwitch : MonoBehaviour
{
    public static Action<bool> OnDimensionChanged;

    [Header("Room Settings")]
    [SerializeField] private GameObject physicRoom;
    [SerializeField] private GameObject digitalRoom;
    [SerializeField] private float upDistanceMult;

    [SerializeField] private GameObject player; // in the future with the generator, once the room is placed we need to get a ref to player.
    private bool isInsideRoom;
    private bool inDigitalWorld;

    public DimensionalSwitch GetDimensionalSwitch()
    {
        return this;
    }

    public void CanSwitchDimension()
    {
        if (isInsideRoom)
        {
            SwitchDimension();
        }
    }

    private void SwitchDimension()
    {
        if (!inDigitalWorld)
        {
            digitalRoom.SetActive(true);
            physicRoom.SetActive(false);

            player.transform.position = player.transform.position + Vector3.up * upDistanceMult;

            inDigitalWorld = true;
        }
        else
        {
            digitalRoom.SetActive(false);
            physicRoom.SetActive(true);

            player.transform.position = player.transform.position + Vector3.down * upDistanceMult;

            inDigitalWorld = false;
        }

        OnDimensionChanged?.Invoke(inDigitalWorld);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isInsideRoom = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isInsideRoom = false;
        }
    }
}
