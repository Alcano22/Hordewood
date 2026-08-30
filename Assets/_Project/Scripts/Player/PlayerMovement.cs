using Hordewood.Input;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Hordewood.Player
{
    [RequireComponent(typeof(Rigidbody2D))]
    public class PlayerMovement : MonoBehaviour
    {
        [SerializeField] private float moveSpeed = 5f;
        [SerializeField] private PlayerAnimatorController animController;

        private Rigidbody2D _rb;
        private PlayerControls _controls;
        private Vector2 _moveInput;

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
        }

        private void OnDisable()
        {
            _controls.Player.Move.performed -= OnMove;
            _controls.Player.Move.canceled -= OnMove;
            _controls.Player.Disable();
        }

        private void OnMove(InputAction.CallbackContext ctx)
        {
            _moveInput = ctx.ReadValue<Vector2>();
        }

        private void FixedUpdate()
        {
            _rb.linearVelocity = _moveInput * moveSpeed;

            if (_moveInput.sqrMagnitude > 0.01f)
            {
                animController.PlayRun();
                animController.SetFacing(_moveInput.x);
            } else
                animController.PlayIdle();
        }
    }
}
