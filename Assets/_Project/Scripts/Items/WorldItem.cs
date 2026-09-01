using System.Collections;
using UnityEngine;

namespace Hordewood.Items
{
    [RequireComponent(typeof(SpriteRenderer))]
    public class WorldItem : PickupBase
    {
        [SerializeField] private ItemData item;

        protected override void Awake()
        {
            base.Awake();

            if (item != null)
                RefreshVisual();
        }

        public void Init(ItemData newItem)
        {
            item = newItem;
            RefreshVisual();
        }

        private void RefreshVisual() => spriteRenderer.sprite = item.Icon;

        protected override void OnCollected()
        {
            InventorySystem.Instance.Add(item);
        }
    }
}
