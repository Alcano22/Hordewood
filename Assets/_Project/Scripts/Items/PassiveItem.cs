using UnityEngine;
using UnityEngine.Localization;
using Hordewood.Player;

namespace Hordewood.Items
{
    [CreateAssetMenu(fileName = "New Passive Item", menuName = "Hordewood/Items/PassiveItem")]
    public class PassiveItem : ScriptableObject
    {
        [SerializeField] private LocalizedString displayName;
        [SerializeField] private Sprite icon;
        [SerializeField] private ItemRarity rarity = ItemRarity.Common;
        [SerializeField] private bool isUnique = false;
        [SerializeField] private StatModifier[] modifiers;

        public string DisplayName => Localization.LocalizationService.Instance.GetString(displayName);
        public Sprite Icon => icon;
        public ItemRarity Rarity => rarity;
        public bool IsUnique => isUnique;
        public StatModifier[] Modifiers => modifiers;
    }
}
