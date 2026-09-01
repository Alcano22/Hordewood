using UnityEngine;
using Hordewood.Combat;

namespace Hordewood.Player
{
    public class PlayerHealth : MonoBehaviour, IDamageable
    {
        [SerializeField] private float baseMaxHealth = 100f;
        [SerializeField] private float invulnerabilityDuration = 0.5f;
        [SerializeField] private PlayerStats stats;

        public float CurrentHealth { get; private set; }
        public float InvulnerabilityTimer { get; private set; }

        public event System.Action<float, float> OnHealthChanged;
        public event System.Action OnDeath;

        private void Awake()
        {
            CurrentHealth = MaxHealth;
        }

        private void OnEnable()
        {
            if (stats != null)
                stats.OnStatsChanged += OnMaxHealthPossiblyChanged;
        }

        private void OnDisable()
        {
            if (stats != null)
                stats.OnStatsChanged -= OnMaxHealthPossiblyChanged;
        }

        private void OnMaxHealthPossiblyChanged()
        {
            CurrentHealth = Mathf.Min(CurrentHealth, MaxHealth);
            OnHealthChanged?.Invoke(CurrentHealth, MaxHealth);
        }

        private void Update()
        {
            if (InvulnerabilityTimer > 0f)
                InvulnerabilityTimer -= Time.deltaTime;
        }

        public void Heal(float amount)
        {
            if (CurrentHealth >= MaxHealth) return;

            CurrentHealth = Mathf.Min(CurrentHealth + amount, MaxHealth);
            OnHealthChanged?.Invoke(CurrentHealth, MaxHealth);
        }

        public void TakeDamage(float amount)
        {
            if (InvulnerabilityTimer > 0f) return;

            CurrentHealth -= amount;
            InvulnerabilityTimer = invulnerabilityDuration;

            OnHealthChanged?.Invoke(CurrentHealth, MaxHealth);

            if (CurrentHealth <= 0f)
                Die();
        }

        private void Die()
        {
            OnDeath?.Invoke();
        }

        public float MaxHealth => stats.GetModifiedValue(StatType.MaxHealth, baseMaxHealth);
    }
}
