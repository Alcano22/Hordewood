using UnityEngine;
using System.Collections.Generic;

namespace Hordewood.World
{
    public class StructureSpawner : MonoBehaviour
    {
        [SerializeField] private StructureSpawnConfig[] configs;
        [SerializeField] private int maxAttemptsPerStructure = 20;

        public HashSet<Vector3Int> GenerateStructures(WorldGenerator worldGenerator)
        {
            var blockedCells = new HashSet<Vector3Int>();
            var placedPositions = new List<Vector2>();

            foreach (var config in configs)
                SpawnConfig(config, worldGenerator, placedPositions, blockedCells);

            return blockedCells;
        }

        private void SpawnConfig(StructureSpawnConfig config,
                                 WorldGenerator worldGenerator,
                                 List<Vector2> placedPositions,
                                 HashSet<Vector3Int> blockedCells)
        {
            int spawned = 0;
            int guard = 0;

            while (spawned < config.Count && guard < config.Count * maxAttemptsPerStructure)
            {
                guard++;

                int x = Random.Range(0, worldGenerator.Width);
                int y = Random.Range(0, worldGenerator.Height);
                Vector3Int cell = new(x - worldGenerator.Width / 2, y - worldGenerator.Height / 2, 0);

                if (!worldGenerator.IsFullyGroundCell(cell)) continue;

                Vector2 worldPos = worldGenerator.CellToWorld(cell);
                if (IsTooClose(worldPos, config.MinDistanceBetween, placedPositions)) continue;

                GameObject instance = Instantiate(config.Prefab, worldPos, Quaternion.identity);

                var occupiedCells = new List<Vector3Int>(GetOccupiedCells(instance, worldGenerator));
                foreach (var occupiedCell in occupiedCells)
                    blockedCells.Add(occupiedCell);

                if (instance.TryGetComponent<Destructible>(out var destructible))
                    destructible.SetOccupiedCells(worldGenerator, occupiedCells);

                placedPositions.Add(worldPos);
                spawned++;
            }
        }

        private static IEnumerable<Vector3Int> GetOccupiedCells(GameObject instance, WorldGenerator worldGenerator)
        {
            Collider2D physicalCollider = FindPhysicalCollider(instance);
            if (physicalCollider == null)
                yield break;

            Bounds bounds = physicalCollider.bounds;
            Vector3Int min = worldGenerator.WorldToCell(bounds.min);
            Vector3Int max = worldGenerator.WorldToCell(bounds.max);

            for (int y = min.y; y <= max.y; y++)
                for (int x = min.x; x <= max.x; x++)
                    yield return new Vector3Int(x, y, 0);
        }

        private static Collider2D FindPhysicalCollider(GameObject instance)
        {
            foreach (var collider in instance.GetComponentsInChildren<Collider2D>())
            {
                if (!collider.isTrigger)
                    return collider;
            }

            return null;
        }

        private static bool IsTooClose(Vector2 pos, float minDistance, List<Vector2> placed)
        {
            foreach (var p in placed)
            {
                if (Vector2.Distance(p, pos) < minDistance)
                    return true;
            }
            return false;
        }
    }
}
