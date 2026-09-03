using System.Collections.Generic;
using UnityEngine;
using Hordewood.Enemies;

namespace Hordewood.Core
{
    [CreateAssetMenu(fileName = "New WaveGenerationConfig", menuName = "Hordewood/WaveGenerationConfig")]
    public class WaveGenerationConfig : ScriptableObject
    {
        [System.Serializable]
        private struct EnemyUnlock
        {
            public Enemy enemyPrefab;
            public int unlockWave;
        }

        [Header("Enemy Pool")]
        [SerializeField] private EnemyUnlock[] enemyUnlocks;

        [Header("Duration")]
        [SerializeField] private float baseDuration = 30f;
        [SerializeField] private AnimationCurve durationMultiplier = AnimationCurve.Linear(0f, 1f, 50f, 1.5f);

        [Header("Spawn Rate")]
        [SerializeField] private float baseSpawnInterval = 2f;
        [SerializeField] private AnimationCurve spawnIntervalMultiplier = AnimationCurve.Linear(0f, 1f, 50f, 0.3f);
        [SerializeField] private float minSpawnInterval = 0.2f;

        [Header("Spawns Per Interval")]
        [SerializeField] private int baseSpawnsPerInterval = 1;
        [SerializeField] private AnimationCurve spawnsPerIntervalMultiplier = AnimationCurve.Linear(0f, 1f, 50f, 4f);

        public WaveData GenerateWave(int waveNumber)
        {
            float duration = baseDuration * durationMultiplier.Evaluate(waveNumber);

            float spawnInterval = Mathf.Max(
                minSpawnInterval,
                baseSpawnInterval * spawnIntervalMultiplier.Evaluate(waveNumber)
            );

            int spawnsPerInterval = Mathf.Max(
                1,
                Mathf.RoundToInt(baseSpawnsPerInterval * spawnsPerIntervalMultiplier.Evaluate(waveNumber))
            );

            Enemy[] enemyPool = GetUnlockedEnemies(waveNumber);
            return new WaveData(duration, enemyPool, spawnInterval, spawnsPerInterval);
        }

        private Enemy[] GetUnlockedEnemies(int waveNumber)
        {
            var list = new List<Enemy>();

            foreach (var unlock in enemyUnlocks)
            {
                if (waveNumber >= unlock.unlockWave)
                    list.Add(unlock.enemyPrefab);
            }

            return list.ToArray();
        }
    }
}
