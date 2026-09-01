using UnityEngine;
using Hordewood.Player;

namespace Hordewood.Items
{
    [CreateAssetMenu(fileName = "New Passive Item", menuName = "Hordewood/Items/PassiveItem")]
    public class PassiveItem : ScriptableObject
    {
        [SerializeField] private string displayName;
        [SerializeField] private Sprite icon;
        [SerializeField] private ItemRarity rarity = ItemRarity.Common;
        [SerializeField] private StatModifier[] modifiers;

        public string DisplayName => displayName;
        public Sprite Icon => icon;
        public ItemRarity Rarity => rarity;
        public StatModifier[] Modifiers => modifiers;
    }
}
