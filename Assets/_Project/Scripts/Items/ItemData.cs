using UnityEngine;

namespace Hordewood.Items
{
    [CreateAssetMenu(fileName = "New Item", menuName = "Hordewood/Items/ItemData")]
    public class ItemData : ScriptableObject
    {
        [SerializeField] private string displayName;
        [SerializeField] private Sprite icon;

        public string DisplayName => displayName;
        public Sprite Icon => icon;
    }
}
