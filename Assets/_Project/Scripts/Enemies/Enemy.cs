using UnityEngine;
using Hordewood.Core;
using Hordewood.Combat;
using Hordewood.Items;

namespace Hordewood.Enemies
{
    [RequireComponent(typeof(Rigidbody2D))]
    public class Enemy : MonoBehaviour, IDamageable
    {
        [SerializeField] private EnemyStats stats;
        [SerializeField] private EnemyAnimatorController animController;
        [SerializeField] private EnemyPathfinder pathfinder;
        [SerializeField] private SpriteTint spriteTint;
        [SerializeField] private ParticleSystem deathEffect;

        [Header("Loot")]
        [SerializeField] private DropTable dropTable;

        private Rigidbody2D _rb;
        private Transform _target;
        private float _currentHealth;

        private void Awake()
        {
            _rb = GetComponent<Rigidbody2D>();
            _currentHealth = stats.MaxHealth;
        }

        private void Start()
        {
            var player = GameObject.FindGameObjectWithTag("Player");
            if (player != null)
                _target = player.transform;
        }

        private void FixedUpdate()
        {
            if (_target == null) return;

            pathfinder.UpdatePath(_rb.position, _target.position);
            Vector2 direction = pathfinder.GetMoveDirection(_rb.position);
            if (direction == Vector2.zero)
            {
                _rb.linearVelocity = Vector2.zero;
                animController.PlayIdle();
                return;
            }

            _rb.linearVelocity = direction * stats.MoveSpeed;
            animController.PlayWalk();
            animController.SetFacing(direction.x);
        }

        public void TakeDamage(float amount)
        {
            _currentHealth -= amount;
            spriteTint.Flash();

            if (_currentHealth <= 0f)
                Die(true);
        }

        public void Kill()
        {
            if (_currentHealth <= 0f) return;

            _currentHealth = 0f;
            Die(false);
        }

        private void Die(bool dropLoot)
        {
            enabled = false;
            _rb.linearVelocity = Vector2.zero;
            _rb.simulated = false;

            animController.PlayDeath(() =>
            {
                if (deathEffect != null)
                {
                    deathEffect.transform.parent = null;
                    deathEffect.Play();
                    Destroy(deathEffect.gameObject, deathEffect.main.duration);
                }

                if (dropLoot)
                    SpawnDrop();
                Destroy(gameObject);
            });
        }

        private void SpawnDrop()
        {
            ItemDropper.Instance.DropFromTable(dropTable, transform.position);
        }

        private void OnCollisionEnter2D(Collision2D collision) => TryDealContactDamage(collision);
        private void OnCollisionStay2D(Collision2D collision) => TryDealContactDamage(collision);

        private void TryDealContactDamage(Collision2D collision)
        {
            if (!collision.gameObject.CompareTag("Player")) return;

            if (collision.gameObject.TryGetComponent<IDamageable>(out var damageable))
                damageable.TakeDamage(stats.ContactDamage);
        }
    }
}
