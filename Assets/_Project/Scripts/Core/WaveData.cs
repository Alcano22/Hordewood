using UnityEngine;
using UnityEngine.Localization;
using Hordewood.Enemies;

namespace Hordewood.Core
{
    [CreateAssetMenu(fileName = "New Wave", menuName = "Hordewood/WaveData")]
    public class WaveData : ScriptableObject
    {
        [SerializeField] private float duration = 30f;
        [SerializeField] private Enemy[] enemyTypes;
        [SerializeField] private float spawnInterval = 1f;
        [SerializeField] private int spawnsPerInterval = 1;

        public float Duration => duration;
        public Enemy[] EnemyTypes => enemyTypes;
        public float SpawnInterval => spawnInterval;
        public int SpawnsPerInterval => spawnsPerInterval;
    }
}
