using UnityEngine;

namespace Hordewood.Player
{
    [RequireComponent(typeof(Animator))]
    public class PlayerAnimatorController : MonoBehaviour
    {
        private const string IdleState = "Player_Idle";
        private const string RunState = "Player_Run";

        [SerializeField] private Color dashTintColor = new(0.6f, 0.85f, 1f);

        private Animator _animator;
        private SpriteRenderer _spriteRenderer;
        private string _currentState;
        private Color _originalColor;

        private void Awake()
        {
            _animator = GetComponent<Animator>();
            _spriteRenderer = GetComponent<SpriteRenderer>();
            _originalColor = _spriteRenderer.color;
        }

        public void PlayIdle() => Play(IdleState);
        public void PlayRun() => Play(RunState);

        private void Play(string state)
        {
            if (_currentState == state) return;

            _animator.Play(state);
            _currentState = state;
        }

        public void SetFacing(float inputX)
        {
            if (inputX > 0.01f)
                _spriteRenderer.flipX = false;
            else if (inputX < -0.01f)
                _spriteRenderer.flipX = true;
        }

        public void SetDashing(bool isDashing)
        {
            _spriteRenderer.color = isDashing ? dashTintColor : _originalColor;
        }
    }
}
