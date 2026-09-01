using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Hordewood.Items;

namespace Hordewood.UI
{
    public class InventorySlotUI : MonoBehaviour
    {
        [SerializeField] private Image icon;
        [SerializeField] private TextMeshProUGUI countText;

        public void SetItem(ItemData item, int count)
        {
            icon.sprite = item.Icon;
            countText.text = count.ToString();
        }
    }
}
