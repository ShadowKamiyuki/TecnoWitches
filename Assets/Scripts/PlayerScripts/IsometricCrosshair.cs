using UnityEngine;

public class IsoCursor : MonoBehaviour
{
    [Header("Cursor Settings")]
    [SerializeField] private Camera mainCamera;
    [SerializeField] private float floorHeightPhysical;
    [SerializeField] private float floorHeightDigital;

    private bool isDigital;
    private PlayerControls controls;

    private void Awake()
    {
        controls = new PlayerControls();
    }

    private void OnEnable()
    {
        controls.Player.Enable();
        DimensionalSwitch.OnDimensionChanged += UpdateDimension;
    }

    private void OnDisable()
    {
        controls.Player.Disable();
        DimensionalSwitch.OnDimensionChanged -= UpdateDimension;
    }

    private void Update()
    {
        UpdateCursorPosition();
    }

    private void UpdateCursorPosition()
    {
        float targetY = isDigital ? floorHeightDigital : floorHeightPhysical;

        // reads the position of the mouse from the input system and creates a ray to the floor
        Vector2 mousePos = controls.Player.Point.ReadValue<Vector2>();
        Ray ray = mainCamera.ScreenPointToRay(mousePos);

        // creates an horizontal plane
        Plane isometricPlane = new Plane(Vector3.up, new Vector3(0, targetY, 0));

        if (isometricPlane.Raycast(ray, out float dist))
        {
            Vector3 hitPoint = ray.GetPoint(dist);
            hitPoint.y = targetY; // where the floor is

            transform.position = hitPoint;
        }
    }

    private void UpdateDimension(bool isDigital)
    {
        this.isDigital = isDigital;
    }
}
