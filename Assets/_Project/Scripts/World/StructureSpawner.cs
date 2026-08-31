using UnityEngine;
using System.Collections.Generic;

namespace Hordewood.World
{
    public class StructureSpawner : MonoBehaviour
    {
        [SerializeField] private WorldGenerator worldGenerator;
        [SerializeField] private GameObject structurePrefab;
        [SerializeField] private int count = 8;
        [SerializeField] private float minDistanceBetween = 15f;
        [SerializeField] private int maxAttemptsPerStructure = 20;

        private void Awake()
        {
            worldGenerator.OnWorldGenerated += SpawnAll;
        }

        private void SpawnAll()
        {
            var placed = new List<Vector2>();
            int spawned = 0;
            int guard = 0;

            while (spawned < count && guard < count * maxAttemptsPerStructure)
            {
                guard++;

                int x = Random.Range(0, worldGenerator.Width);
                int y = Random.Range(0, worldGenerator.Height);
                Vector3Int cell = new(x - worldGenerator.Width / 2, y - worldGenerator.Height / 2, 0);

                if (!worldGenerator.IsFullyGroundCell(cell)) continue;

                Vector2 worldPos = worldGenerator.CellToWorld(cell);
                if (IsTooClose(worldPos, placed)) continue;

                Instantiate(structurePrefab, worldPos, Quaternion.identity);
                placed.Add(worldPos);
                spawned++;
            }
        }
        
        private bool IsTooClose(Vector2 pos, List<Vector2> placed)
        {
            foreach (var p in placed)
            {
                if (Vector2.Distance(p, pos) < minDistanceBetween)
                    return true;
            }
            return false;
        }
    }
}
