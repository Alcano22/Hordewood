using System.Collections.Generic;
using UnityEngine;
using Hordewood.Core;
using Hordewood.Combat;
using Hordewood.Items;

namespace Hordewood.World
{
    [RequireComponent(typeof(SpriteRenderer))]
    public class Destructible : MonoBehaviour, IDamageable
    {
        [SerializeField] private float maxHealth = 15f;
        [SerializeField] private DropTable dropTable;
        [SerializeField] private ParticleSystem destroyEffect;
        [SerializeField] private SpriteTint spriteTint;

        private float _currentHealth;
        private bool _isDestroyed;
        private WorldGenerator _worldGenerator;
        private List<Vector3Int> _occupiedCells;

        private void Awake()
        {
            _currentHealth = maxHealth;
        }

        public void SetOccupiedCells(WorldGenerator worldGenerator, List<Vector3Int> cells)
        {
            _worldGenerator = worldGenerator;
            _occupiedCells = cells;
        }

        public void TakeDamage(float amount)
        {
            if (_isDestroyed) return;

            _currentHealth -= amount;
            spriteTint.Flash();

            if (_currentHealth <= 0f)
                Die();
        }

        private void Die()
        {
            if (_isDestroyed) return;
            _isDestroyed = true;

            if (destroyEffect != null)
            {
                destroyEffect.transform.parent = null;
                destroyEffect.Play();
                Destroy(destroyEffect.gameObject, destroyEffect.main.duration);
            }

            ItemDropper.Instance.DropFromTable(dropTable, transform.position);

            if (_worldGenerator != null && _occupiedCells != null)
            {
                foreach (var cell in _occupiedCells)
                    _worldGenerator.SetBlocked(cell, false);
            }

            Destroy(gameObject);
        }
    }
}
