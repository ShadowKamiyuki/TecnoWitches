using UnityEngine;
using UnityEngine.InputSystem;

public class InputHandler : MonoBehaviour, IUpdatable
{
    private PlayerControls controls;
    public PlayerControls Controls => controls;

    private ICommand moveCommand;
    private ICommand dodgeCommand;
    private ICommand attackCommand;
    private ICommand pauseCommand;
    private ICommand specialAttackCommand;
    private ICommand switchDimensionCommand;
    private ICommand interactCommand;

    [HideInInspector] public Vector2 moveDir;

    [SerializeField] private float switchCooldown;

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
        dodgeCommand = new DodgeCommand(playerActions);
        attackCommand = new ShootCommand(playerActions);
        pauseCommand = new PauseCommand();
        specialAttackCommand = new ShootSpecialCommand(playerActions);
        switchDimensionCommand = new SwitchCommand(playerActions, switchCooldown);
        interactCommand = new InteractCommand(playerActions);

        IsometricCrosshair crosshair = FindAnyObjectByType<IsometricCrosshair>();
        crosshair.Init(controls);
    }

    private void OnEnable()
    {
        controls.Player.Enable();

        // Suscribimos eventos
        controls.Player.Attack.performed += OnAttack;
        controls.Player.Dodge.performed += OnDodge;
        controls.Player.Pause.performed += OnPause;
        controls.Player.SpecialAttack.performed += OnSpecialAttack;
        controls.Player.SwitchDimension.performed += OnDimensionSwitch;
        controls.Player.Interact.performed += OnInteraction;
    }

    private void OnDisable()
    {
        // Limpieza
        controls.Player.Attack.performed -= OnAttack;
        controls.Player.Dodge.performed -= OnDodge;
        controls.Player.Pause.performed -= OnPause;
        controls.Player.SpecialAttack.performed -= OnSpecialAttack;
        controls.Player.SwitchDimension.performed -= OnDimensionSwitch;
        controls.Player.Interact.performed -= OnInteraction;

        controls.Player.Disable();
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
        if (ServiceLocator.Get<GameManager>().currentState.gameState != GameManager.GameState.Gameplay)
            return;

        InputManagement();
    }

    private void InputManagement()
    {
        if (ServiceLocator.Get<GameManager>().currentState.gameState != GameManager.GameState.Gameplay)
            return;

        moveDir = controls.Player.Move.ReadValue<Vector2>().normalized;

        moveCommand.Execute();
    }

    private void OnAttack(InputAction.CallbackContext context)
    {
        attackCommand.Execute();
    }

    private void OnDodge(InputAction.CallbackContext context)
    {
        dodgeCommand.Execute();
    }

    private void OnPause(InputAction.CallbackContext context)
    {
        pauseCommand.Execute();
    }

    private void OnSpecialAttack(InputAction.CallbackContext context)
    {
        specialAttackCommand.Execute();
    }

    private void OnDimensionSwitch(InputAction.CallbackContext context)
    {
        switchDimensionCommand.Execute();
    }

    private void OnInteraction(InputAction.CallbackContext context)
    {
        interactCommand.Execute();
    }
}
