using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using Hordewood.Items;
using Hordewood.Input;

namespace Hordewood.UI
{
    public class InventoryUI : MonoBehaviour
    {
        [SerializeField] private GameObject panel;
        [SerializeField] private InventorySlotUI slotPrefab;
        [SerializeField] private Transform slotContainer;

        private readonly Dictionary<ItemData, InventorySlotUI> _slots = new();
        private PlayerControls _controls;

        private void Awake() => panel.SetActive(false);

        private void OnEnable()
        {
            _controls = PlayerControlsProvider.Instance.Controls;
            _controls.Player.ToggleInventory.performed += OnToggleInventory;
        }

        private void Start()
        {
            if (InventorySystem.Instance != null)
                InventorySystem.Instance.OnItemCountChanged += UpdateSlot;
        }

        private void OnDisable()
        {
            _controls.Player.ToggleInventory.performed -= OnToggleInventory;
        }

        private void OnDestroy()
        {
            if (InventorySystem.Instance != null)
                InventorySystem.Instance.OnItemCountChanged -= UpdateSlot;
        }

        private void OnToggleInventory(InputAction.CallbackContext context)
        {
            panel.SetActive(!panel.activeSelf);
        }

        private void UpdateSlot(ItemData item, int count)
        {
            if (!_slots.TryGetValue(item, out var slot))
            {
                slot = Instantiate(slotPrefab, slotContainer);
                _slots[item] = slot;
            }

            slot.SetItem(item, count);
        }
    }
}
