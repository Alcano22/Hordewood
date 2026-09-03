using Hordewood.Enemies;

namespace Hordewood.Core
{
    public class WaveData
    {
        public float Duration { get; }
        public Enemy[] EnemyTypes { get; }
        public float SpawnInterval { get; }
        public int SpawnsPerInterval { get; }

        public WaveData(float duration, Enemy[] enemyTypes, float spawnInterval, int spawnsPerInterval)
        {
            Duration = duration;
            EnemyTypes = enemyTypes;
            SpawnInterval = spawnInterval;
            SpawnsPerInterval = spawnsPerInterval;
        }
    }
}
