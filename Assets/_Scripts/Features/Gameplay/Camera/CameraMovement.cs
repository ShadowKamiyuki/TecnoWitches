using UnityEngine;

public class CameraMovement : MonoBehaviour
{
    [SerializeField] private Transform target;
    [SerializeField] private Vector3 offset;

    public void SetCameraTarget(Transform target)
    {
        this.target = target;
    }

    private void LateUpdate()
    {
        transform.position = target.position + offset;
    }
}
