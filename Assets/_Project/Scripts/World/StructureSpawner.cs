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
            var reservedCells = new HashSet<Vector3Int>();

            foreach (var config in configs)
                SpawnConfig(config, worldGenerator, reservedCells, blockedCells);

            return blockedCells;
        }

        private void SpawnConfig(StructureSpawnConfig config,
                                 WorldGenerator worldGenerator,
                                 HashSet<Vector3Int> reservedCells,
                                 HashSet<Vector3Int> blockedCells)
        {
            Bounds relativeClearance = MeasureRelativeClearance(config.Prefab);

            int spawned = 0;
            int guard = 0;

            while (spawned < config.Count && guard < config.Count * maxAttemptsPerStructure)
            {
                guard++;

                int x = Random.Range(0, worldGenerator.Width);
                int y = Random.Range(0, worldGenerator.Height);
                Vector3Int originCell = new(x - worldGenerator.Width / 2, y - worldGenerator.Height / 2, 0);
                Vector2 worldPos = worldGenerator.CellToWorld(originCell);

                if (!TryGetClearanceCells(worldPos, relativeClearance, worldGenerator, 
                                          reservedCells, out var clearanceCells)) continue;

                GameObject instance = Instantiate(config.Prefab, worldPos, Quaternion.identity);

                var occupiedCells = new List<Vector3Int>(GetOccupiedCells(instance, worldGenerator));
                foreach (var cell in occupiedCells)
                    blockedCells.Add(cell);

                foreach (var cell in clearanceCells)
                    reservedCells.Add(cell);

                if (instance.TryGetComponent<Destructible>(out var destructible))
                    destructible.SetOccupiedCells(worldGenerator, occupiedCells);

                spawned++;
            }
        }

        private static Bounds MeasureRelativeClearance(GameObject prefab)
        {
            if (!prefab.TryGetComponent<StructureBase>(out var structureBase))
                return new Bounds(Vector3.zero, Vector3.one);

            return structureBase.ClearanceBounds;
        }

        private static bool TryGetClearanceCells(Vector2 worldPos,
                                                 Bounds relativeClearance,
                                                 WorldGenerator worldGenerator,
                                                 HashSet<Vector3Int> reservedCells,
                                                 out List<Vector3Int> clearanceCells)
        {
            Vector2 center = worldPos + (Vector2)relativeClearance.center;
            Bounds worldBounds = new(center, relativeClearance.size);

            clearanceCells = new List<Vector3Int>(GetCellsInBounds(worldBounds, worldGenerator));

            foreach (var cell in clearanceCells)
            {
                if (!worldGenerator.IsFullyGroundCell(cell))
                    return false;
                if (reservedCells.Contains(cell))
                    return false;
            }

            return true;
        }

        private static IEnumerable<Vector3Int> GetCellsInBounds(Bounds bounds, WorldGenerator worldGenerator)
        {
            Vector3Int min = worldGenerator.WorldToCell(bounds.min);
            Vector3Int max = worldGenerator.WorldToCell(bounds.max);

            for (int y = min.y; y <= max.y; y++)
                for (int x = min.x; x <= max.x; x++)
                    yield return new Vector3Int(x, y, 0);
        }

        private static IEnumerable<Vector3Int> GetOccupiedCells(GameObject instance, WorldGenerator worldGenerator)
        {
            Collider2D physicalCollider = FindPhysicalCollider(instance);
            if (physicalCollider == null)
                yield break;

            foreach (var cell in GetCellsInBounds(physicalCollider.bounds, worldGenerator))
                yield return cell;
        }

        private static Collider2D FindPhysicalCollider(GameObject instance)
        {
            foreach (var collider in instance.GetComponentsInChildren<Collider2D>())
            {
                if (collider.enabled && !collider.isTrigger)
                    return collider;
            }

            return null;
        }
    }
}
