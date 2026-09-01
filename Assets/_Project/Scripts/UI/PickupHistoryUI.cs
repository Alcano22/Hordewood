using System.Collections.Generic;
using UnityEngine;
using Hordewood.Items;

namespace Hordewood.UI
{
    public class PickupHistoryUI : MonoBehaviour
    {
        [SerializeField] private PickupPopupUI popupPrefab;
        [SerializeField] private Transform popupContainer;
        [SerializeField] private Sprite coinSprite;
        [SerializeField] private int maxVisiblePopups = 6;

        private readonly Dictionary<Sprite, PickupPopupUI> _activePopups = new();
        private readonly Queue<Sprite> _popupOrder = new();

        private void Start()
        {
            if (InventorySystem.Instance != null)
                InventorySystem.Instance.OnItemAdded += OnItemAdded;
            if (CurrencySystem.Instance != null)
                CurrencySystem.Instance.OnCoinsAdded += OnCoinsAdded;
        }

        private void OnDestroy()
        {
            if (InventorySystem.Instance != null)
                InventorySystem.Instance.OnItemAdded -= OnItemAdded;
            if (CurrencySystem.Instance != null)
                CurrencySystem.Instance.OnCoinsAdded -= OnCoinsAdded;
        }

        private void OnItemAdded(ItemData item, int amount) => SpawnOrUpdatePopup(item.Icon, amount, item.DisplayName);
        private void OnCoinsAdded(int amount) => SpawnOrUpdatePopup(coinSprite, amount, "Coin");

        private void SpawnOrUpdatePopup(Sprite sprite, int amount, string displayName)
        {
            if (_activePopups.TryGetValue(sprite, out var existing) && existing != null)
            {
                existing.AddAmount(amount);
                return;
            }

            if (_activePopups.Count >= maxVisiblePopups)
            {
                Sprite oldestSprite = _popupOrder.Dequeue();
                if (_activePopups.TryGetValue(oldestSprite, out var oldest) && oldest != null)
                    Destroy(oldest.gameObject);
                _activePopups.Remove(oldestSprite);
            }

            PickupPopupUI popup = Instantiate(popupPrefab, popupContainer);
            popup.Setup(sprite, amount, displayName);

            _activePopups[sprite] = popup;
            _popupOrder.Enqueue(sprite);
        }
    }
}
