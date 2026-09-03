using UnityEngine;
using Hordewood.Core;
using Hordewood.Combat;
using Hordewood.Items;
using Hordewood.UI;
using UnityEngine.Rendering;

namespace Hordewood.Enemies
{
    [RequireComponent(typeof(Rigidbody2D))]
    public class Enemy : MonoBehaviour, IDamageable, IHealthSource
    {
        [SerializeField] private EnemyStats stats;
        [SerializeField] private EnemyAnimatorController animController;
        [SerializeField] private EnemyPathfinder pathfinder;
        [SerializeField] private EnemyMovementBase movement;
        [SerializeField] private SpriteTint spriteTint;
        [SerializeField] private HealthBarUI healthBar;
        [SerializeField] private ParticleSystem deathEffect;
        [SerializeField] private bool dealsContactDamage = true;
        [SerializeField] private EnemyRangedAttack rangedAttack;

        [Header("Loot")]
        [SerializeField] private DropTable dropTable;

        public event System.Action<float, float> OnHealthChanged;
        public float CurrentHealth => _currentHealth;
        public float MaxHealth => stats.MaxHealth;

        private Rigidbody2D _rb;
        private Transform _target;
        private float _currentHealth;
        private bool _isDead;

        private void Awake()
        {
            _rb = GetComponent<Rigidbody2D>();
            _currentHealth = stats.MaxHealth;
            movement.Init(_rb, animController, pathfinder, stats);

            if (healthBar != null)
                healthBar.SetHealthSource(this);
        }

        private void Start()
        {
            var player = GameObject.FindGameObjectWithTag("Player");
            if (player != null)
                _target = player.transform;
        }

        private void FixedUpdate()
        {
            if (_target != null)
                movement.UpdateMovement(_target);
        }

        public void TakeDamage(float amount)
        {
            if (_isDead) return;

            _currentHealth -= amount;
            spriteTint.Flash();
            OnHealthChanged?.Invoke(_currentHealth, stats.MaxHealth);

            if (_currentHealth <= 0f)
                Die(true);
        }

        public void Kill()
        {
            if (_isDead) return;

            _currentHealth = 0f;
            Die(false);
        }

        private void Die(bool dropLoot)
        {
            if (_isDead) return;
            _isDead = true;

            enabled = false;
            movement.StopMovement();

            if (rangedAttack != null)
                rangedAttack.enabled = false;

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

        private void OnTriggerEnter2D(Collider2D other) => TryDealContactDamage(other);
        private void OnTriggerStay2D(Collider2D other) => TryDealContactDamage(other);

        private void TryDealContactDamage(Collider2D other)
        {
            if (!dealsContactDamage) return;
            if (!other.CompareTag("Player")) return;

            if (other.TryGetComponent<IDamageable>(out var damageable))
                damageable.TakeDamage(stats.ContactDamage);
        }
    }
}
