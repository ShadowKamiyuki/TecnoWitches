using UnityEngine;

public class IsoCursor : MonoBehaviour
{
    [Header("Cursor Settings")]
    [SerializeField] private Camera mainCamera;
    [SerializeField] private LayerMask floorMask;   // FloorLow, FloorHigh
    [SerializeField] private float cursorHeightOffset = 0.05f;

    private PlayerControls controls;

    private void Awake()
    {
        controls = new PlayerControls();
    }

    private void OnEnable()
    {
        controls.Player.Enable();
    }

    private void OnDisable()
    {
        controls.Player.Disable();
    }

    private void Update()
    {
        UpdateCursorPosition();
    }

    private void UpdateCursorPosition()
    {
        Vector2 mousePos = controls.Player.Point.ReadValue<Vector2>();
        Ray ray = mainCamera.ScreenPointToRay(mousePos);

        if (Physics.Raycast(ray, out RaycastHit hit, 100f, floorMask))
        {
            Vector3 cursorPos = hit.point;
            cursorPos.y += cursorHeightOffset;

            transform.position = cursorPos;
        }
    }
}
