using UnityEngine;
using Hordewood.Weapons;

namespace Hordewood.Enemies
{
    public class EnemyRangedAttack : MonoBehaviour
    {
        [SerializeField] private Transform firePoint;
        [SerializeField] private float damage = 8f;
        [SerializeField] private float fireInterval = 2f;
        [SerializeField] private float projectileSpeed = 6f;
        [SerializeField] private float range = 7f;
        [SerializeField] private float maxTravelDistance = 10f;

        private Transform _target;
        private float _fireTimer;

        private void Start()
        {
            var player = GameObject.FindGameObjectWithTag("Player");
            if (player != null)
                _target = player.transform;
        }

        private void Update()
        {
            if (_target == null) return;

            AimAtTarget();

            _fireTimer -= Time.deltaTime;

            float distance = Vector2.Distance(transform.position, _target.position);
            if (distance > range) return;

            if (_fireTimer <= 0f)
            {
                Fire();
                _fireTimer = fireInterval;
            }
        }

        private void AimAtTarget()
        {
            Vector2 direction = ((Vector2)_target.position - (Vector2)firePoint.position).normalized;
            firePoint.right = direction;
        }

        private void Fire()
        {
            Vector2 direction = firePoint.right;
            Bullet bullet = EnemyBulletPool.Instance.Get(firePoint.position, Quaternion.identity);
            bullet.Init(direction, projectileSpeed, damage, maxTravelDistance);
        }
    }
}
