using UnityEngine;

namespace Hordewood.Enemies
{
    public class WalkingMovement : EnemyMovementBase
    {
        public override void UpdateMovement(Transform target)
        {
            Pathfinder.UpdatePath(Rigidbody.position, target.position);
            Vector2 direction = Pathfinder.GetMoveDirection(Rigidbody.position);

            if (direction == Vector2.zero)
            {
                Rigidbody.linearVelocity = Vector2.zero;
                AnimController.PlayIdle();
                return;
            }

            Rigidbody.linearVelocity = direction * Stats.MoveSpeed;
            AnimController.PlayWalk();
            AnimController.SetFacing(direction.x);
        }
    }
}
