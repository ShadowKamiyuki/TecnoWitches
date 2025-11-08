using UnityEngine;

public class IsometricCrosshair : MonoBehaviour, IUpdatable
{
    [Header("References")]
    [SerializeField] private Camera cam;
    [SerializeField] private Transform crosshair;
    [SerializeField] private LayerMask groundMask;

    private PlayerControls controls;

    public void Init(PlayerControls controlsRef)
    {
        controls = controlsRef;
    }

    private void Start()
    {
        ServiceLocator.Get<CustomUpdateManager>().Register(this);
    }

    private void OnDestroy()
    {
        ServiceLocator.Get<CustomUpdateManager>()?.Unregister(this);
    }

    public void Tick(float deltaTime)
    {
        MoveCrosshair();
        ShowCrosshair();
    }

    private void ShowCrosshair()
    {
        if (ServiceLocator.Get<GameManager>().currentState.gameState != GameManager.GameState.Gameplay)
        {
            crosshair.gameObject.SetActive(false);
        }
        else
        {
            crosshair.gameObject.SetActive(true);
        }
    }

    private void MoveCrosshair()
    {
        Vector2 mousePos = controls.Player.Point.ReadValue<Vector2>();
        Ray ray = cam.ScreenPointToRay(mousePos);

        if (Physics.Raycast(ray, out RaycastHit hit, Mathf.Infinity, groundMask))
        {
            Vector3 pos = hit.point;
            pos.y += 0.01f;
            crosshair.position = pos;
        }
    }
}
