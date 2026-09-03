using UnityEngine;
using UnityEngine.Localization;
using UnityEngine.Tilemaps;
using Hordewood.Localization;

namespace Hordewood.World
{
    [CreateAssetMenu(fileName = "New Biome", menuName = "Hordewood/World/BiomeData")]
    public class BiomeData : ScriptableObject
    {
        [SerializeField] private LocalizedString displayName;
        [SerializeField] private TileBase groundTile;

        public string GetDisplayName() => LocalizationService.Instance.GetString(displayName);
        public TileBase GroundTile => groundTile;
    }
}
