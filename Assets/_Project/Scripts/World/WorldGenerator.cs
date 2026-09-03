using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using Hordewood.Core;

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
        [SerializeField] private StructureSpawner structureSpawner;
        [SerializeField] private BiomeManager biomeManager;

        [Header("Size")]
        [SerializeField] private int width = 100;
        [SerializeField] private int height = 100;

        [Header("Noise")]
        [SerializeField] private float noiseScale = 10f;
        [SerializeField] private float waterThreshold = 0.4f;

        public event System.Action OnWorldGenerated;

        private readonly HashSet<Vector3Int> _blockedCells = new();
        private bool[,] _groundGrid;

        private TileBase GroundTile => biomeManager.CurrentBiome.GroundTile;

        private void OnEnable() => biomeManager.OnBiomeChanged += OnBiomeChanged;
        private void OnDisable() => biomeManager.OnBiomeChanged -= OnBiomeChanged;

        private void Start() => Generate();

        private void OnBiomeChanged(BiomeData newBiome) => Generate();

        [ContextMenu("Generate World")]
        public void Generate()
        {
            ClearWorld();

            _groundGrid = new bool[width, height];

            float offsetX = GameSeed.CurrentSeed * 10000f;
            float offsetY = GameSeed.CurrentSeed * 5000f;

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
                    tilemap.SetTile(cellPos, GroundTile);
                    _groundGrid[x, y] = true;
                }
            }

            if (structureSpawner != null)
            {
                var occupiedCells = structureSpawner.GenerateStructures(this);
                foreach (var cell in occupiedCells)
                    _blockedCells.Add(cell);
            }

            OnWorldGenerated?.Invoke();
        }

        [ContextMenu("Clear World")]
        public void ClearWorld()
        {
            tilemap.ClearAllTiles();
            _blockedCells.Clear();
        }

        public void SetBlocked(Vector3Int cellPos, bool blocked)
        {
            if (blocked)
                _blockedCells.Add(cellPos);
            else
                _blockedCells.Remove(cellPos);
        }

        public bool IsBlocked(Vector3Int cellPos) => _blockedCells.Contains(cellPos);

        public bool HasGroundTile(Vector3Int cellPos)
        {
            if (!TryToGridIndex(cellPos, out int gx, out int gy))
                return false;

            return _groundGrid[gx, gy];
        }

        public bool IsGroundCell(Vector3Int cellPos) => HasGroundTile(cellPos) && !IsBlocked(cellPos);
        public bool IsGround(Vector2 worldPos) => IsGroundCell(WorldToCell(worldPos));

        public bool IsFullyGroundTile(Vector3Int cellPos)
        {
            if (!HasGroundTile(cellPos))
                return false;

            foreach (var offset in Neighbors)
            {
                if (!HasGroundTile(cellPos + offset))
                    return false;
            }

            return true;
        }

        public bool IsFullyGroundCell(Vector3Int cellPos) => IsFullyGroundTile(cellPos) && !IsBlocked(cellPos);
        public bool IsFullyGround(Vector2 worldPos) => IsFullyGroundCell(WorldToCell(worldPos));

        private bool TryToGridIndex(Vector3Int cellPos, out int gx, out int gy)
        {
            gx = cellPos.x + width / 2;
            gy = cellPos.y + height / 2;
            return _groundGrid != null && gx >= 0 && gx < width && gy >= 0 && gy < height;
        }

        public Vector3Int WorldToCell(Vector2 worldPos) => tilemap.WorldToCell(worldPos);
        public Vector2 CellToWorld(Vector3Int cellPos) => tilemap.GetCellCenterWorld(cellPos);

        public int Width => width;
        public int Height => height;
    }
}
