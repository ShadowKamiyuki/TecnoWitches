using UnityEngine;

public class IsometricCrosshair : MonoBehaviour, IUpdatable
{
    [SerializeField] private Sprite[] cursorSprites;
    [SerializeField] private Camera mainCamera;
    [SerializeField] private float floorHeight = 0f;

    private SpriteRenderer spriteRenderer;
    private IAppStateMachine _stateMachine;
    private IInputService input;

    public Vector3 CursorPosition => transform.position;

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    private void OnEnable()
    {
        _stateMachine = ServiceLocator.Get<IAppStateMachine>();
        input = ServiceLocator.Get<IInputService>();

        input.AttackStarted += HandleAttackStarted;
        input.AttackCanceled += HandleAttackCanceled;

        ServiceLocator.Get<IUpdateService>()?.Register(this);
    }

    private void OnDisable()
    {
        if (input != null)
        {
            input.AttackStarted -= HandleAttackStarted;
            input.AttackCanceled -= HandleAttackCanceled;
        }

        ServiceLocator.Get<IUpdateService>()?.Unregister(this);
    }

    public void Tick(float deltaTime)
    {
        if (_stateMachine.CurrentState != AppState.Gameplay)
            return;

        UpdateCursorPosition();
    }

    private void UpdateCursorPosition()
    {
        Vector2 mousePos = input.MousePosition;
        Ray ray = mainCamera.ScreenPointToRay(mousePos);

        Plane plane = new Plane(Vector3.up, new Vector3(0, floorHeight, 0));

        if (plane.Raycast(ray, out float distance))
        {
            Vector3 hitPoint = ray.GetPoint(distance);
            hitPoint.y = floorHeight;
            transform.position = hitPoint;
        }
    }

    private void HandleAttackStarted()
    {
        if (_stateMachine.CurrentState != AppState.Gameplay)
            return;

        SetCursorState(CursorState.Clicked);
    }

    private void HandleAttackCanceled()
    {
        if (_stateMachine.CurrentState != AppState.Gameplay)
            return;

        SetCursorState(CursorState.Default);
    }

    public void SetCursorState(CursorState state)
    {
        spriteRenderer.sprite = cursorSprites[(int)state];
    }
}