using System;
using System.Collections;
using UnityEngine;

namespace Hordewood.Enemies
{
    [RequireComponent(typeof(Animator))]
    [RequireComponent(typeof(SpriteRenderer))]
    public class EnemyAnimatorController : MonoBehaviour
    {
        [SerializeField] private string animationPrefix;
        [SerializeField] private float corpseLingerTime = 1f;

        private Animator _animator;
        private SpriteRenderer _spriteRenderer;
        private string _currentState;

        private void Awake()
        {
            _animator = GetComponent<Animator>();
            _spriteRenderer = GetComponent<SpriteRenderer>();
        }

        public void PlayIdle() => Play("Idle");
        public void PlayWalk() => Play("Walk");

        public void PlayDeath(Action onComplete)
        {
            Play("Death");
            StartCoroutine(WaitForClip("Death", onComplete));
        }

        private void Play(string suffix)
        {
            string state = $"{animationPrefix}_{suffix}";
            if (_currentState == state) return;

            _animator.Play(state);
            _currentState = state;
        }

        private IEnumerator WaitForClip(string suffix, Action onComplete)
        {
            string state = $"{animationPrefix}_{suffix}";
            float length = GetClipLength(state);

            yield return new WaitForSeconds(length + corpseLingerTime);
            onComplete?.Invoke();
        }

        private float GetClipLength(string stateName)
        {
            foreach (var clip in _animator.runtimeAnimatorController.animationClips)
            {
                if (clip.name == stateName)
                    return clip.length;
            }

            return 0f;
        }

        public void SetFacing(float directionX)
        {
            if (directionX > 0.01f)
                _spriteRenderer.flipX = false;
            if (directionX < -0.01f)
                _spriteRenderer.flipX = true;
        }
    }
}
