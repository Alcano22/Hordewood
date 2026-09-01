using Hordewood.Core;
using UnityEngine;

namespace Hordewood.Items
{
    public class ItemDropper : Singleton<ItemDropper>
    {
        [SerializeField] private WorldItem worldItemPrefab;
        [SerializeField] private WorldCoin worldCoinPrefab;

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

            if (dropTable.TryRollDrop(out ItemData item, out int itemAmount))
                Drop(item, itemAmount, position);

            if (dropTable.TryRollCoins(out int coinAmount))
                DropCoins(coinAmount, position);
        }
    }
}
