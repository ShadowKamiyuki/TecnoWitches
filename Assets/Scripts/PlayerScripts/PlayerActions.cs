using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class PlayerActions : MonoBehaviour
{
    private Rigidbody rb;
    private PlayerStats player;
    private Vector3 move3D;
    private DimensionalSwitch currentRoom;
    [HideInInspector] public DimensionalSwitch CurrentRoom => currentRoom;
    private GameManager gm;

    private void Start()
    {
        player = GetComponent<PlayerStats>();
        rb = GetComponent<Rigidbody>();
        gm = ServiceLocator.Get<GameManager>();
    }

    private void FixedUpdate()
    {
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

    public void Dodge()
    {
        Debug.Log("Player jumped!");
    }

    public void Attack()
    {
        Debug.Log("Player attacked!");
    }

    public void SpecialAttack()
    {
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
