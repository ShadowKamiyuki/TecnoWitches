using UnityEngine;

public class Chest : MonoBehaviour, IInteractable
{
    private bool opened = false;

    public void Interact(IInteractor player)
    {
        if (opened) return;

        opened = true;
        Debug.Log("¡Cofre abierto!");
    }
}
