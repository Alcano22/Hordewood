using Hordewood.Core;
using System.Collections.Generic;

namespace Hordewood.Items
{
    public class InventorySystem : Singleton<InventorySystem>
    {
        private readonly Dictionary<ItemData, int> _counts = new();

        public event System.Action<ItemData, int> OnItemCountChanged;
        public event System.Action<ItemData, int> OnItemAdded;

        public void Add(ItemData item, int amount = 1)
        {
            _counts.TryGetValue(item, out int current);
            _counts[item] = current + amount;
            OnItemCountChanged?.Invoke(item, _counts[item]);
            OnItemAdded?.Invoke(item, amount);
        }

        public int GetCount(ItemData item) => _counts.TryGetValue(item, out int count) ? count : 0;
    }
}
