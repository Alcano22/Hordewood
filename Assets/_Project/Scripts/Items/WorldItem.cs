using UnityEngine;

namespace Hordewood.Items
{
    [RequireComponent(typeof(SpriteRenderer))]
    public class WorldItem : MonoBehaviour
    {
        [SerializeField] private ItemData item;
        [SerializeField] private int amount;

        private SpriteRenderer _spriteRenderer;

        private void Awake()
        {
            _spriteRenderer = GetComponent<SpriteRenderer>();
            RefreshVisual();
        }

        public void Init(ItemData newItem, int newAmount = 1)
        {
            item = newItem;
            amount = newAmount;
            RefreshVisual();
        }

        private void RefreshVisual() => _spriteRenderer.sprite = item.Icon;

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (!other.CompareTag("Player")) return;

            InventorySystem.Instance.Add(item, amount);
            Destroy(gameObject);
        }
    }
}
