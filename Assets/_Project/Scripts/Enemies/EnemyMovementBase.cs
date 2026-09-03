using UnityEngine;

namespace Hordewood.Enemies
{
    public abstract class EnemyMovementBase : MonoBehaviour
    {
        protected Rigidbody2D Rigidbody { get; private set; }
        protected EnemyAnimatorController AnimController { get; private set; }
        protected EnemyPathfinder Pathfinder { get; private set; }
        protected EnemyStats Stats { get; private set; }

        public void Init(Rigidbody2D rb, 
                         EnemyAnimatorController animController, 
                         EnemyPathfinder pathfinder, 
                         EnemyStats stats)
        {
            Rigidbody = rb;
            AnimController = animController;
            Pathfinder = pathfinder;
            Stats = stats;
        }

        public abstract void UpdateMovement(Transform target);

        public virtual void StopMovement() => Rigidbody.linearVelocity = Vector2.zero;
    }
}
