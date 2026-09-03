using System.Collections;
using UnityEngine;

namespace Hordewood.Enemies
{
    public class LeapMovement : EnemyMovementBase
    {
        [SerializeField] private float leapInterval = 2f;
        [SerializeField] private float leapDistance = 3f;
        [SerializeField] private float leapArcHeight = 0.6f;
        [SerializeField] private float leapDuration = 0.35f;

        private float _leapTimer;
        private bool _isLeaping;

        public override void UpdateMovement(Transform target)
        {
            if (_isLeaping) return;

            AnimController.PlayIdle();

            _leapTimer -= Time.fixedDeltaTime;
            if (_leapTimer > 0f) return;

            TryStartLeap(target);
        }

        private void TryStartLeap(Transform target)
        {
            Vector2 direction = ((Vector2)target.position - Rigidbody.position).normalized;
            Vector2 candidate = Rigidbody.position + direction * leapDistance;

            if (!Pathfinder.IsValidLandingSpot(candidate))
            {
                _leapTimer = leapInterval * 0.25f;
                return;
            }

            AnimController.SetFacing(direction.x);
            StartCoroutine(LeapRoutine(Rigidbody.position, candidate));
        }

        private IEnumerator LeapRoutine(Vector2 start, Vector2 end)
        {
            _isLeaping = true;
            Rigidbody.linearVelocity = Vector2.zero;
            AnimController.PlayIdle();

            float elapsed = 0f;
            while (elapsed < leapDuration)
            {
                elapsed += Time.fixedDeltaTime;
                float t = Mathf.Clamp01(elapsed / leapDuration);

                Vector2 groundPos = Vector2.Lerp(start, end, t);
                float arc = Mathf.Sin(t * Mathf.PI) * leapArcHeight;

                Rigidbody.MovePosition(groundPos + Vector2.up * arc);
                yield return new WaitForFixedUpdate();
            }

            Rigidbody.MovePosition(end);
            _isLeaping = false;
            _leapTimer = leapInterval;
        }

        public override void StopMovement()
        {
            base.StopMovement();

            StopAllCoroutines();
            _isLeaping = false;
        }
    }
}
