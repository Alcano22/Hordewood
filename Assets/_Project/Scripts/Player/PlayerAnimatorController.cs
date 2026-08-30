using UnityEngine;

namespace Hordewood.Player
{
    [RequireComponent(typeof(Animator))]
    public class PlayerAnimatorController : MonoBehaviour
    {
        private const string IdleState = "Player_Idle";
        private const string RunState = "Player_Run";

        private Animator _animator;
        private SpriteRenderer _spriteRenderer;
        private string _currentState;

        private void Awake()
        {
            _animator = GetComponent<Animator>();
            _spriteRenderer = GetComponent<SpriteRenderer>();
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
    }
}
