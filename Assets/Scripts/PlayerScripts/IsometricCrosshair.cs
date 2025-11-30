using UnityEngine;

public class IsometricCrosshair : MonoBehaviour, IUpdatable
{
    [Header("Cursor Settings")]
    [SerializeField] private Camera mainCamera;
    [SerializeField] private float floorHeightPhysical;
    [SerializeField] private float floorHeightDigital;

    // internal variables
    private bool isDigital;
    private PlayerControls controls;
    private GameManager gm;

    // properties
    public Vector3 CursorPosition => transform.position;

    private void Awake()
    {
        controls = new PlayerControls();
    }

    private void OnEnable()
    {
        controls.Player.Enable();
        ServiceLocator.Get<CustomUpdateManager>().Register(this);
        gm = ServiceLocator.Get<GameManager>();

        DimensionalSwitch.OnDimensionChanged += UpdateDimension;
    }

    private void OnDisable()
    {
        controls.Player.Disable();
        CustomUpdateManager updateManager = ServiceLocator.Get<CustomUpdateManager>();

        if (updateManager != null)
            updateManager.Unregister(this);

        DimensionalSwitch.OnDimensionChanged -= UpdateDimension;
    }

    public void Tick(float deltaTime)
    {
        if (gm.currentState.gameState == GameManager.GameState.Gameplay)
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
