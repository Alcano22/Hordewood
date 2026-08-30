using UnityEngine;
using System;
using Hordewood.Combat;

namespace Hordewood.Player
{
    public class PlayerHealth : MonoBehaviour, IDamageable
    {
        [SerializeField] private float maxHealth = 100f;
        [SerializeField] private float invulnerabilityDuration = 0.5f;

        public float CurrentHealth { get; private set; }
        public float InvulnerabilityTimer { get; private set; }

        public event Action<float, float> OnHealthChanged;
        public event Action OnDeath;

        public float MaxHealth => maxHealth;

        private void Awake()
        {
            CurrentHealth = maxHealth;
        }

        private void Update()
        {
            if (InvulnerabilityTimer > 0f)
                InvulnerabilityTimer -= Time.deltaTime;
        }

        public void TakeDamage(float amount)
        {
            if (InvulnerabilityTimer > 0f) return;

            CurrentHealth -= amount;
            InvulnerabilityTimer = invulnerabilityDuration;

            OnHealthChanged?.Invoke(CurrentHealth, maxHealth);

            if (CurrentHealth <= 0f)
                Die();
        }

        private void Die()
        {
            OnDeath?.Invoke();
        }
    }
}
