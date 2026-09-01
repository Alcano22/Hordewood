using UnityEngine;

namespace Hordewood.Items
{
    [CreateAssetMenu(fileName = "New Drop Table", menuName = "Hordewood/Items/DropTable")]
    public class DropTable : ScriptableObject
    {
        [System.Serializable]
        private struct DropEntry
        {
            public ItemData item;
            [Range(0f, 1f)] public float dropChance;
            public int minAmount;
            public int maxAmount;
        }

        [SerializeField] private DropEntry[] entries;

        [Header("Coins")]
        [Range(0f, 1f), SerializeField] private float coinDropChance;
        [SerializeField] private int minCoins;
        [SerializeField] private int maxCoins;

        public bool TryRollDrop(out ItemData item, out int amount)
        {
            foreach (var entry in entries)
            {
                if (Random.value > entry.dropChance) continue;

                item = entry.item;
                amount = Random.Range(entry.minAmount, entry.maxAmount + 1);
                return true;
            }

            item = null;
            amount = 0;
            return false;
        }

        public bool TryRollCoins(out int amount)
        {
            if (Random.value > coinDropChance)
            {
                amount = 0;
                return false;
            }

            amount = Random.Range(minCoins, maxCoins + 1);
            return true;
        }
    }
}
