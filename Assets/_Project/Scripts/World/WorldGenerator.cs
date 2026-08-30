using UnityEngine;
using UnityEngine.Tilemaps;

namespace Hordewood.World
{
    public class WorldGenerator : MonoBehaviour
    {
        private static readonly Vector3Int[] Neighbors =
        {
            new(1, 0, 0), new(-1,  0, 0), new( 0, 1, 0), new( 0, -1, 0),
            new(1, 1, 0), new( 1, -1, 0), new(-1, 1, 0), new(-1, -1, 0)
        };

        [SerializeField] private Tilemap tilemap;
        [SerializeField] private TileBase grassTile;

        [Header("Size")]
        [SerializeField] private int width = 100;
        [SerializeField] private int height = 100;

        [Header("Noise")]
        [SerializeField] private float noiseScale = 10f;
        [SerializeField] private float waterThreshold = 0.4f;
        [SerializeField] private int seed;

        private void Start() => Generate();

        [ContextMenu("Generate World")]
        public void Generate()
        {
            ClearWorld();

            float offsetX = seed * 10000f;
            float offsetY = seed * 5000f;

            for (int y = 0; y < height; y++)
            {
                for (int x = 0; x < width; x++)
                {
                    float noiseValue = Mathf.PerlinNoise(
                        (x + offsetX) / noiseScale,
                        (y + offsetY) / noiseScale
                    );

                    if (noiseValue < waterThreshold) continue;

                    Vector3Int cellPos = new(x - width / 2, y - height / 2, 0);
                    tilemap.SetTile(cellPos, grassTile);
                }
            }
        }

        [ContextMenu("Clear World")]
        public void ClearWorld() => tilemap.ClearAllTiles();

        public bool IsGroundCell(Vector3Int cellPos) => tilemap.GetTile(cellPos) != null;
        public bool IsGround(Vector2 worldPos) => IsGroundCell(WorldToCell(worldPos));

        public bool IsFullyGroundCell(Vector3Int cellPos)
        {
            if (!IsGroundCell(cellPos))
                return false;

            foreach (var offset in Neighbors)
            {
                if (!IsGroundCell(cellPos + offset))
                    return false;
            }

            return true;
        }
        public bool IsFullyGround(Vector2 worldPos) => IsFullyGroundCell(WorldToCell(worldPos));

        public Vector3Int WorldToCell(Vector2 worldPos) => tilemap.WorldToCell(worldPos);
        public Vector2 CellToWorld(Vector3Int cellPos) => tilemap.GetCellCenterWorld(cellPos);
    }
}
