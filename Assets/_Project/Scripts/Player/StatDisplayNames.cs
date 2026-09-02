using UnityEngine;
using UnityEngine.Localization;
using Hordewood.Localization;

namespace Hordewood.Player
{
    [CreateAssetMenu(fileName = "New StatDisplayNames", menuName = "Hordewood/Player/StatDisplayNames")]
    public class StatDisplayNames : ScriptableObject
    {
        [System.Serializable]
        private struct Entry
        {
            public StatType Type;
            public LocalizedString DisplayName;
        }

        [SerializeField] private Entry[] entries;

        public string GetDisplayName(StatType type)
        {
            foreach (var entry in entries)
            {
                if (entry.Type == type)
                    return LocalizationService.Instance.GetString(entry.DisplayName);
            }

            return type.ToString();
        }
    }
}
