using System.Collections.Generic;
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

        public readonly struct DropResult
        {
            public readonly ItemData Item;
            public readonly int Amount;

            public DropResult(ItemData item, int amount)
            {
                Item = item;
                Amount = amount;
            }
        }

        [SerializeField] private DropEntry[] entries;

        [Header("Coins")]
        [Range(0f, 1f), SerializeField] private float coinDropChance;
        [SerializeField] private int minCoins;
        [SerializeField] private int maxCoins;

        public void RollDrops(List<DropResult> results)
        {
            results.Clear();

            foreach (var entry in entries)
            {
                if (Random.value > entry.dropChance) continue;

                int amount = Random.Range(entry.minAmount, entry.maxAmount + 1);
                results.Add(new DropResult(entry.item, amount));
            }
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
