using UnityEngine;
using Hordewood.Combat;

namespace Hordewood.Weapons
{
    [RequireComponent(typeof(Rigidbody2D))]
    public class Bullet : MonoBehaviour
    {
        [SerializeField] private ParticleSystem hitEffect;

        private float _damage;
        private float _maxDistance;
        private Vector2 _startPosition;

        public void Init(Vector2 direction, float speed, float damage, float maxDistance)
        {
            _damage = damage;
            _maxDistance = maxDistance;
            _startPosition = transform.position;

            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
            transform.rotation = Quaternion.Euler(0f, 0f, angle);

            var rb = GetComponent<Rigidbody2D>();
            rb.linearVelocity = direction * speed;
        }

        private void Update()
        {
            if (Vector2.Distance(_startPosition, transform.position) >= _maxDistance)
                DestroyBullet();
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (other.TryGetComponent<IDamageable>(out var damageable))
                damageable.TakeDamage(_damage);

            DestroyBullet();
        }

        private void DestroyBullet()
        {
            if (hitEffect != null)
            {
                hitEffect.transform.parent = null;
                hitEffect.Play();
                Destroy(hitEffect.gameObject, hitEffect.main.duration);
            }

            Destroy(gameObject);
        }
    }
}
