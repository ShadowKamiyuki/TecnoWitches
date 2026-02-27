using UnityEngine;
using UnityEngine.InputSystem;

public class InputHandler : MonoBehaviour, IUpdatable
{
    private PlayerControls controls;
    private bool isAttackHeld;

    private ICommand moveCommand;
    private ICommand dodgeCommand;
    private ICommand attackCommand;
    private ICommand pauseCommand;
    private ICommand specialAttackCommand;
    private ICommand switchDimensionCommand;
    private ICommand interactCommand;
    private ICommand previousCommand;
    private ICommand nextCommand;

    [HideInInspector] public Vector2 moveDir;

    [SerializeField] private float switchCooldown;
    [SerializeField] private float dodgeCooldown;
    [SerializeField] private IsometricCrosshair crosshair;

    private void Awake()
    {
        controls = new PlayerControls();

        ServiceLocator.Get<CustomUpdateManager>().Register(this);
    }

    private void Start()
    {
        PlayerActions playerActions = FindAnyObjectByType<PlayerActions>();

        if (playerActions == null)
        {
            Debug.LogError("[InputHandler] : PlayerMovement not found in scene");
            enabled = false;
            return;
        }

        moveCommand = new MoveCommand(playerActions, this);
        dodgeCommand = new DodgeCommand(playerActions, dodgeCooldown);
        attackCommand = new ShootCommand(playerActions);
        pauseCommand = new PauseCommand();
        specialAttackCommand = new ShootSpecialCommand(playerActions);
        switchDimensionCommand = new SwitchCommand(playerActions, switchCooldown);
        interactCommand = new InteractCommand(playerActions);
        nextCommand = new NextCommand(playerActions);
        previousCommand = new PreviousCommand(playerActions);
    }

    private void OnEnable()
    {
        //controls.Player.Enable();

        //// Suscribimos eventos
        //controls.Player.Attack.started += OnAttackStarted;
        //controls.Player.Attack.canceled += OnAttackCanceled;
        //controls.Player.Dodge.performed += OnDodge;
        //controls.Player.Pause.performed += OnPause;
        //controls.Player.SpecialAttack.performed += OnSpecialAttack;
        //controls.Player.SwitchDimension.performed += OnDimensionSwitch;
        //controls.Player.Interact.performed += OnInteraction;
        //controls.Player.Next.performed += OnNextSpell;
        //controls.Player.Previous.performed += OnPreviousSpell;
    }

    private void OnDisable()
    {
        // Limpieza
        //controls.Player.Attack.started -= OnAttackStarted;
        //controls.Player.Attack.canceled -= OnAttackCanceled;
        //controls.Player.Dodge.performed -= OnDodge;
        //controls.Player.Pause.performed -= OnPause;
        //controls.Player.SpecialAttack.performed -= OnSpecialAttack;
        //controls.Player.SwitchDimension.performed -= OnDimensionSwitch;
        //controls.Player.Interact.performed -= OnInteraction;
        //controls.Player.Next.performed -= OnNextSpell;
        //controls.Player.Previous.performed -= OnPreviousSpell;

        //controls.Player.Disable();
    }

    private void OnDestroy()
    {
        CustomUpdateManager updateManager = ServiceLocator.Get<CustomUpdateManager>();

        if (updateManager != null)
        {
            updateManager.Unregister(this);
        }
    }

    public void Tick(float deltaTime)
    {
        //if (ServiceLocator.Get<GameManager>().CurrentState.gameState != GameManager.GameState.Gameplay)
        //    return;

        InputManagement();
        HandleAttackInput();
    }

    private void HandleAttackInput()
    {
        if (!isAttackHeld)
            return;

        attackCommand.Execute(); // internamente llama a RuntimeSpell.StartCast()
    }

    private void InputManagement()
    {
        //if (ServiceLocator.Get<GameManager>().CurrentState.gameState != GameManager.GameState.Gameplay)
        //    return;

        moveDir = controls.Gameplay.Move.ReadValue<Vector2>().normalized;

        moveCommand.Execute();
    }

    private void OnAttackStarted(InputAction.CallbackContext context)
    {
        //if (ServiceLocator.Get<GameManager>().CurrentState.gameState != GameManager.GameState.Gameplay)
        //    return;

        isAttackHeld = true;
        crosshair.SetCursorState(CursorState.Clicked);
    }

    private void OnAttackCanceled(InputAction.CallbackContext context)
    {
        //if (ServiceLocator.Get<GameManager>().CurrentState.gameState != GameManager.GameState.Gameplay)
        //    return;

        isAttackHeld = false;
        crosshair.SetCursorState(CursorState.Default);
    }

    private void OnDodge(InputAction.CallbackContext context)
    {
        //if (ServiceLocator.Get<GameManager>().CurrentState.gameState != GameManager.GameState.Gameplay)
        //    return;

        dodgeCommand.Execute();
    }

    private void OnPause(InputAction.CallbackContext context)
    {
        //GameManager.GameState gameState = ServiceLocator.Get<GameManager>().CurrentState.gameState;

        //if (gameState == GameManager.GameState.Gameplay || gameState == GameManager.GameState.Paused)
        //{
        //    pauseCommand.Execute();
        //}
    }

    private void OnSpecialAttack(InputAction.CallbackContext context)
    {
        //if (ServiceLocator.Get<GameManager>().CurrentState.gameState != GameManager.GameState.Gameplay)
        //    return;

        specialAttackCommand.Execute();
    }

    private void OnDimensionSwitch(InputAction.CallbackContext context)
    {
        //if (ServiceLocator.Get<GameManager>().CurrentState.gameState != GameManager.GameState.Gameplay)
        //    return;

        switchDimensionCommand.Execute();
    }

    private void OnInteraction(InputAction.CallbackContext context)
    {
        //if (ServiceLocator.Get<GameManager>().CurrentState.gameState != GameManager.GameState.Gameplay)
        //    return;

        interactCommand.Execute();
    }

    private void OnNextSpell(InputAction.CallbackContext context)
    {
        //if (ServiceLocator.Get<GameManager>().CurrentState.gameState != GameManager.GameState.Gameplay)
        //    return;

        nextCommand.Execute();
    }

    private void OnPreviousSpell(InputAction.CallbackContext context)
    {
        //if (ServiceLocator.Get<GameManager>().CurrentState.gameState != GameManager.GameState.Gameplay)
        //    return;

        previousCommand.Execute();
    }
}
