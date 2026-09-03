using UnityEngine;
using Hordewood.Core;
using Hordewood.Combat;

namespace Hordewood.Weapons
{
    [RequireComponent(typeof(Rigidbody2D))]
    public class Bullet : MonoBehaviour, IPoolable
    {
        [SerializeField] private ParticleSystem hitEffectPrefab;
        [SerializeField] private TrailRenderer trailRenderer;

        private Rigidbody2D _rb;
        private IBulletPool _ownerPool;
        private float _damage;
        private float _maxDistance;
        private Vector2 _startPosition;

        private void Awake() => _rb = GetComponent<Rigidbody2D>();

        public void SetOwnerPool(IBulletPool pool) => _ownerPool = pool;

        public void Init(Vector2 direction, float speed, float damage, float maxDistance)
        {
            _damage = damage;
            _maxDistance = maxDistance;
            _startPosition = transform.position;

            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
            transform.rotation = Quaternion.Euler(0f, 0f, angle);

            _rb.linearVelocity = direction * speed;
        }

        private void Update()
        {
            if (Vector2.Distance(_startPosition, transform.position) >= _maxDistance)
                Despawn();
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (other.TryGetComponent<IDamageable>(out var damageable))
                damageable.TakeDamage(_damage);

            Despawn();
        }

        private void Despawn()
        {
            if (hitEffectPrefab != null)
            {
                ParticleSystem effect = Instantiate(hitEffectPrefab, transform.position, Quaternion.identity);
                effect.Play();
                Destroy(effect.gameObject, effect.main.duration);
            }

            _ownerPool.Release(this);
        }

        public void OnSpawned()
        {
            if (trailRenderer != null)
                trailRenderer.Clear();
        }

        public void OnDespawned() => _rb.linearVelocity = Vector2.zero;
    }
}
