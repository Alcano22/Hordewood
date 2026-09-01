using UnityEngine;
using UnityEngine.InputSystem;
using Hordewood.Input;
using Hordewood.Core;

namespace Hordewood.Player
{
    [RequireComponent(typeof(Rigidbody2D))]
    public class PlayerMovement : MonoBehaviour
    {
        [SerializeField] private float baseMoveSpeed = 5f;
        [SerializeField] private PlayerAnimatorController animController;
        [SerializeField] private SpriteTint spriteTint;
        [SerializeField] private PlayerStats stats;

        [Header("Dash")]
        [SerializeField] private float dashSpeed = 15f;
        [SerializeField] private float dashDuration = 0.15f;
        [SerializeField] private float dashCooldown = 0.8f;
        [SerializeField] private Color dashTintColor = new(0.6f, 0.85f, 1f);

        private Rigidbody2D _rb;
        private PlayerControls _controls;
        private Vector2 _moveInput;
        private Vector2 _lastMoveDirection = Vector2.right;

        private bool _isDashing;
        private float _dashTimer;
        private float _dashCooldownTimer;

        private void Awake()
        {
            _rb = GetComponent<Rigidbody2D>();
            _controls = new PlayerControls();
        }

        private void OnEnable()
        {
            _controls.Player.Enable();
            _controls.Player.Move.performed += OnMove;
            _controls.Player.Move.canceled += OnMove;
            _controls.Player.Dash.performed += OnDash;
        }

        private void OnDisable()
        {
            _controls.Player.Move.performed -= OnMove;
            _controls.Player.Move.canceled -= OnMove;
            _controls.Player.Dash.performed -= OnDash;
            _controls.Player.Disable();
        }

        private void OnMove(InputAction.CallbackContext ctx)
        {
            _moveInput = ctx.ReadValue<Vector2>();
            if (_moveInput.sqrMagnitude > 0.01f)
                _lastMoveDirection = _moveInput;
        }

        private void OnDash(InputAction.CallbackContext ctx)
        {
            if (_isDashing || _dashCooldownTimer > 0f) return;

            _isDashing = true;
            _dashTimer = dashDuration;
            _dashCooldownTimer = dashCooldown;
            spriteTint.SetTint(dashTintColor, true);
        }

        private void FixedUpdate()
        {
            if (_dashCooldownTimer > 0f)
                _dashCooldownTimer -= Time.fixedDeltaTime;

            if (_isDashing)
            {
                _dashTimer -= Time.fixedDeltaTime;
                _rb.linearVelocity = _lastMoveDirection.normalized * dashSpeed;

                if (_dashTimer <= 0f)
                {
                    _isDashing = false;
                    spriteTint.SetTint(dashTintColor, false);
                }

                return;
            }

            float effectiveMoveSpeed = stats.GetModifiedValue(StatType.MoveSpeed, baseMoveSpeed);
            _rb.linearVelocity = _moveInput * effectiveMoveSpeed;

            if (_moveInput.sqrMagnitude > 0.01f)
            {
                animController.PlayRun();
                animController.SetFacing(_moveInput.x);
            } else
                animController.PlayIdle();
        }
    }
}
