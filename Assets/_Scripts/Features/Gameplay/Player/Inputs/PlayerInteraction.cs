using UnityEngine;

public class PlayerInteraction : MonoBehaviour, IInteractor
{
    private IInteractable _currentInteractable;

    public Transform Transform => transform;

    public void Interact()
    {
        _currentInteractable?.Interact(this);
    }

    private void OnTriggerEnter(Collider other)
    {
        _currentInteractable = other.GetComponent<IInteractable>();
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.GetComponent<IInteractable>() == _currentInteractable)
            _currentInteractable = null;
    }
}