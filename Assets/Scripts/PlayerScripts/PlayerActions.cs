using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class PlayerActions : MonoBehaviour
{
    private Rigidbody rb;
    private PlayerStats player;
    private Vector3 move3D;
    private DimensionalSwitch currentRoom;
    private GameManager gm;
    private PlayerEnergy playerEnergy;

    [HideInInspector] public DimensionalSwitch CurrentRoom => currentRoom;

    [Header("Dash Settings")]
    [SerializeField] private float dashForce = 20f; // force applied to player while dashing (multiplies movement direction)
    [SerializeField] private float dashDuration = 0.15f; // total dash duration time
    [SerializeField] private float dashCooldown = 1f;

    // internal variables
    private bool isDashing = false;
    private float dashTimer = 0f;
    private float dashCooldownTimer = 0f;

    private void Start()
    {
        player = GetComponent<PlayerStats>();
        rb = GetComponent<Rigidbody>();
        playerEnergy = GetComponent<PlayerEnergy>();
        gm = ServiceLocator.Get<GameManager>();
    }

    private void FixedUpdate()
    {
        DodgeMovement();
        Move();
    }

    public void SetMoveDirection(Vector2 direction)
    {
        move3D = new Vector3(direction.x, 0, direction.y).ToIso();
    }

    private void Move()
    {
        if (gm.isGameOver)
        {
            StopMovement();
            return;
        }

        if (player == null)
        {
            Debug.LogError("PlayerStats no encontrado en el objeto del jugador.");
            return;
        }

        rb.velocity = move3D * player.CurrentMoveSpeed; // the player moves with rigidbody
    }

    public void StopMovement()
    {
        move3D = Vector3.zero;
        rb.velocity = Vector3.zero;
    }

    private void DodgeMovement()
    {
        // Si estamos en dash, ignoramos el movimiento normal
        if (isDashing)
        {
            rb.AddForce(move3D * dashForce, ForceMode.VelocityChange);

            dashTimer -= Time.fixedDeltaTime;
            if (dashTimer <= 0f)
                isDashing = false;

            return;
        }

        // Cooldown
        if (dashCooldownTimer > 0f)
            dashCooldownTimer -= Time.fixedDeltaTime;
    }

    public void Dodge()
    {
        if (dashCooldownTimer > 0f)
            return;

        if (move3D == Vector3.zero)
            return; // evitar dash parado

        isDashing = true;
        dashTimer = dashDuration;
        dashCooldownTimer = dashCooldown;

        GetComponent<PlayerHealth>()?.ActivateInvincibility(dashDuration);

        Debug.Log("Player dodged!");
    }

    public void Attack()
    {
        Debug.Log("Player attacked!");
    }

    public void SpecialAttack()
    {
        playerEnergy.UseEnergy(100f);
        Debug.Log("Special attack!");
    }

    public void SwitchDimension()
    {
        if (currentRoom == null)
            return; // avoids a null input while outside the input trigger zone

        currentRoom.CanSwitchDimension();

        Debug.Log("Switched dimensions!");
    }

    public void Interact()
    {
        Debug.Log("Interacted");
    }

    public void SwitchSpell()
    {
        Debug.Log("Switch Spell");
    }

    // triggers when the player is inside a room with dimensional switch
    private void OnTriggerEnter(Collider other)
    {
        DimensionalSwitch ds = other.GetComponent<DimensionalSwitch>();
        if (ds != null)
        {
            currentRoom = ds;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        DimensionalSwitch ds = other.GetComponent<DimensionalSwitch>();
        if (ds != null && currentRoom == ds)
        {
            currentRoom = null;
        }
    }
}
