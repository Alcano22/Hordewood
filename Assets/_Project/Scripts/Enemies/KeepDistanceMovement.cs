using UnityEngine;

namespace Hordewood.Enemies
{
    public class KeepDistanceMovement : EnemyMovementBase
    {
        [SerializeField] private float retreatDistance = 3f;
        [SerializeField] private float approachDistance = 6f;
        [SerializeField] private float fleeTargetDistance = 4f;

        public override void UpdateMovement(Transform target)
        {
            float distance = Vector2.Distance(Rigidbody.position, target.position);

            Vector2 destination;

            if (distance < retreatDistance)
            {
                Vector2 awayDirection = (Rigidbody.position - (Vector2)target.position).normalized;
                destination = Rigidbody.position + awayDirection * fleeTargetDistance;
            } else if (distance > approachDistance)
                destination = target.position;
            else
            {
                Rigidbody.linearVelocity = Vector2.zero;
                AnimController.PlayIdle();
                FaceTarget(target);
                return;
            }

            Pathfinder.UpdatePath(Rigidbody.position, destination);
            Vector2 direction = Pathfinder.GetMoveDirection(Rigidbody.position);
            if (direction == Vector2.zero)
            {
                Rigidbody.linearVelocity = Vector2.zero;
                AnimController.PlayIdle();
                return;
            }

            Rigidbody.linearVelocity = direction * Stats.MoveSpeed;
            AnimController.PlayIdle();
            AnimController.SetFacing(direction.x);
        }

        private void FaceTarget(Transform target)
        {
            AnimController.SetFacing(target.position.x - Rigidbody.position.x);
        }
    }
}
