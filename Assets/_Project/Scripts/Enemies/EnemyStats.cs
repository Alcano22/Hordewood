using UnityEngine;

namespace Hordewood.Enemies
{
    [CreateAssetMenu(fileName = "New Enemy", menuName = "Hordewood/Enemies/EnemyStats")]
    public class EnemyStats : ScriptableObject
    {
        [Header("Info")]
        [SerializeField] private string displayName;

        [Header("Stats")]
        [SerializeField] private float maxHealth = 20f;
        [SerializeField] private float moveSpeed = 2f;
        [SerializeField] private float contactDamage = 5f;

        public string DisplayName => displayName;
        public float MaxHealth => maxHealth;
        public float MoveSpeed => moveSpeed;
        public float ContactDamage => contactDamage;
    }
}
