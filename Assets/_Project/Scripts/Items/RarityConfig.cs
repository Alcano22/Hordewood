using UnityEngine;

namespace Hordewood.Items
{
    [CreateAssetMenu(fileName = "New Rarity Config", menuName = "Hordewood/Items/RarityConfig")]
    public class RarityConfig : ScriptableObject
    {
        [System.Serializable]
        private struct RarityEntry
        {
            public ItemRarity Rarity;
            public float Weight;
            public int BasePrice;
        }

        [SerializeField] private RarityEntry[] entries;

        public float GetWeight(ItemRarity rarity)
        {
            foreach (var entry in entries)
            {
                if (entry.Rarity == rarity)
                    return entry.Weight;
            }
            return 0f;
        }

        public int GetBasePrice(ItemRarity rarity)
        {
            foreach (var entry in entries)
            {
                if (entry.Rarity == rarity)
                    return entry.BasePrice;
            }
            return 0;
        }
    }
}
