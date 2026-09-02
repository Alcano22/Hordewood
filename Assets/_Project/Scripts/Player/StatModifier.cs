
namespace Hordewood.Player
{
    [System.Serializable]
    public struct StatModifier
    {
        public StatType type;
        public float value;
        public bool isPercentage;

        public bool IsPositive => value >= 0f;

        public string GetDescription(StatDisplayNames statNames)
        {
            float displayValue = isPercentage ? value * 100f : value;

            string signedValue = displayValue >= 0
                               ? $"+{displayValue:0.#}"
                               : $"{displayValue:0.#}";

            string valueText = isPercentage ? $"<b>{signedValue}%</b>" : $"<b>{signedValue}</b>";
            return $"{valueText} {statNames.GetDisplayName(type)}";
        }
    }
}
