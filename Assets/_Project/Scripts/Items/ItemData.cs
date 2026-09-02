using UnityEngine;
using UnityEngine.Localization;

namespace Hordewood.Items
{
    public enum ItemRarity { Common, Uncommon, Rare, Legendary }

    public static class ItemRarityExtensions
    {
        public static Color GetColor(this ItemRarity rarity) => rarity switch
        {
            ItemRarity.Common => new Color(0.75f, 0.75f, 0.75f),
            ItemRarity.Uncommon => new Color(0.3f, 0.85f, 0.3f),
            ItemRarity.Rare => new Color(0.3f, 0.55f, 1f),
            ItemRarity.Legendary => new Color(1f, 0.65f, 0.1f),
            _ => Color.white
        };
    }

    [CreateAssetMenu(fileName = "New Item", menuName = "Hordewood/Items/ItemData")]
    public class ItemData : ScriptableObject
    {
        [SerializeField] private LocalizedString displayName;
        [SerializeField] private ItemRarity rarity = ItemRarity.Common;
        [SerializeField] private Sprite icon;

        public string DisplayName => Localization.LocalizationService.Instance.GetString(displayName);
        public ItemRarity Rarity => rarity;
        public Sprite Icon => icon;
    }
}
