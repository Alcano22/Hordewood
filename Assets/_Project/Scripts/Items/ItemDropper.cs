using System.Collections.Generic;
using Hordewood.Core;
using UnityEngine;

namespace Hordewood.Items
{
    public class ItemDropper : Singleton<ItemDropper>
    {
        [SerializeField] private WorldItem worldItemPrefab;
        [SerializeField] private WorldCoin worldCoinPrefab;

        private readonly List<DropTable.DropResult> _dropResultsBuffer = new();

        public void Drop(ItemData item, int amount, Vector2 position)
        {
            for (int i = 0; i < amount; i++)
            {
                WorldItem drop = Instantiate(worldItemPrefab, position, Quaternion.identity);
                drop.Init(item);
            }
        }

        public void DropCoins(int amount, Vector2 position)
        {
            for (int i = 0; i < amount; i++)
                Instantiate(worldCoinPrefab, position, Quaternion.identity);
        }

        public void DropFromTable(DropTable dropTable, Vector2 position)
        {
            if (dropTable == null) return;

            dropTable.RollDrops(_dropResultsBuffer);
            foreach (var result in _dropResultsBuffer)
                Drop(result.Item, result.Amount, position);

            if (dropTable.TryRollCoins(out int coinAmount))
                DropCoins(coinAmount, position);
        }
    }
}
