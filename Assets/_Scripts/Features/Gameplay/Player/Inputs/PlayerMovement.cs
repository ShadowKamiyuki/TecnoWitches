using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private float dashForce = 20f;
    [SerializeField] private float dashDuration = 0.15f;

    private Rigidbody _rb;
    private PlayerStatsRuntime _stats;

    private Vector3 _moveDirection;
    private bool _isDashing;
    private float _dashTimer;
    private float _dashCooldown;

    private void Awake()
    {
        _rb = GetComponent<Rigidbody>();
    }

    private void FixedUpdate()
    {
        HandleDash();
        HandleMovement();
    }

    public void InjectStats(PlayerStatsRuntime stats)
    {
        _stats = stats;
    }

    public void SetMoveDirection(Vector2 direction)
    {
        _moveDirection = new Vector3(direction.x, 0, direction.y).ToIso();
    }

    private void HandleMovement()
    {
        if (_isDashing)
            return;

        _rb.velocity = _moveDirection * _stats.Speed.Value;
    }

    public bool TryDodge(float cooldown)
    {
        if (Time.time < _dashCooldown)
            return false;

        if (_moveDirection == Vector3.zero)
            return false;

        _isDashing = true;
        _dashTimer = dashDuration;
        _dashCooldown = Time.time + cooldown;

        return true;
    }

    private void HandleDash()
    {
        if (!_isDashing)
            return;

        _rb.AddForce(_moveDirection * dashForce, ForceMode.VelocityChange);

        _dashTimer -= Time.fixedDeltaTime;

        if (_dashTimer <= 0f)
            _isDashing = false;
    }
}