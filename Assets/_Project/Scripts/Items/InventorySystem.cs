using System;
using System.Collections.Generic;
using UnityEngine;

namespace Hordewood.Items
{
    public class InventorySystem : MonoBehaviour
    {
        public static InventorySystem Instance { get; private set; }

        private readonly Dictionary<ItemData, int> _counts = new();

        public event Action<ItemData, int> OnItemCountChanged;

        private void Awake()
        {
            if (Instance != null && Instance != this)
            {
                Destroy(gameObject);
                return;
            }
            Instance = this;
        }

        public void Add(ItemData item, int amount = 1)
        {
            _counts.TryGetValue(item, out int current);
            _counts[item] = current + amount;
            OnItemCountChanged?.Invoke(item, _counts[item]);
        }

        public int GetCount(ItemData item) => _counts.TryGetValue(item, out int count) ? count : 0;
    }
}
