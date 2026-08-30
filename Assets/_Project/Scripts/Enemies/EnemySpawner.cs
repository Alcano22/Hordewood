using UnityEngine;
using Hordewood.Core;
using Hordewood.World;

namespace Hordewood.Enemies
{
    public class EnemySpawner : MonoBehaviour
    {
        [SerializeField] private WaveManager waveManager;
        [SerializeField] private Transform player;
        [SerializeField] private float spawnRadius = 8f;

        [SerializeField] private WorldGenerator worldGenerator;
        [SerializeField] private int maxSpawnAttempts = 10;

        private float _spawnTimer;

        private void Update()
        {
            if (GameManager.Instance.CurrentState != GameState.Playing) return;

            WaveData wave = waveManager.CurrentWave;
            if (wave == null) return;

            _spawnTimer -= Time.deltaTime;
            if (_spawnTimer <= 0f)
            {
                for (int i = 0; i < wave.SpawnsPerInterval; i++)
                    SpawnEnemy(wave);

                _spawnTimer = wave.SpawnInterval;
            }
        }

        private void SpawnEnemy(WaveData wave)
        {
            if (!TryGetValidSpawnPosition(out Vector2 spawnPos)) return;

            Enemy prefab = wave.EnemyTypes[Random.Range(0, wave.EnemyTypes.Length)];
            Enemy enemy = Instantiate(prefab, spawnPos, Quaternion.identity);
            enemy.GetComponent<EnemyPathfinder>().SetWorldGenerator(worldGenerator);
        }

        private bool TryGetValidSpawnPosition(out Vector2 result)
        {
            for (int i = 0; i < maxSpawnAttempts; i++)
            {
                Vector2 candidate = GetRandomPointAroundPlayer();
                if (!worldGenerator.IsFullyGround(candidate)) continue;

                result = candidate;
                return true;
            }

            result = Vector2.zero;
            return false;
        }

        private Vector2 GetRandomPointAroundPlayer()
        {
            Vector2 randomDirection = Random.insideUnitCircle.normalized;
            return (Vector2)player.position + randomDirection * spawnRadius;
        }
    }
}
