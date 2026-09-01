using System.Collections.Generic;
using UnityEngine;
using Hordewood.Items;

namespace Hordewood.Player
{
    public class PlayerStats : MonoBehaviour
    {
        private readonly List<PassiveItem> _activeItems = new();
        private readonly Dictionary<StatType, float> _flatBonus = new();
        private readonly Dictionary<StatType, float> _percentBonus = new();

        public event System.Action OnStatsChanged;

        public void AddPassive(PassiveItem item)
        {
            _activeItems.Add(item);

            foreach (var mod in item.Modifiers)
            {
                if (mod.isPercentage)
                {
                    _percentBonus.TryGetValue(mod.type, out float current);
                    _percentBonus[mod.type] = current + mod.value;
                } else
                {
                    _flatBonus.TryGetValue(mod.type, out float current);
                    _flatBonus[mod.type] = current + mod.value;
                }
            }

            OnStatsChanged?.Invoke();
        }

        public float GetModifiedValue(StatType type, float baseValue)
        {
            _flatBonus.TryGetValue(type, out float flat);
            _percentBonus.TryGetValue(type, out float percent);

            return (baseValue + flat) * (1f + percent);
        }

        public IReadOnlyList<PassiveItem> ActiveItems => _activeItems;
    }
}
