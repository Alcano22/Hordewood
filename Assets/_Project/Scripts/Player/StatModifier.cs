
namespace Hordewood.Player
{
    [System.Serializable]
    public struct StatModifier
    {
        public StatType type;
        public float value;
        public bool isPercentage;

        public bool IsPositive => value >= 0f;

        public string GetDescription()
        {
            string sign = value >= 0 ? "+" : "";
            string valueText = isPercentage ? $"{sign}{value:0.#}%" : $"{sign}{value:0.#}";
            return $"{valueText} {type.GetDisplayName()}";
        }
    }
}
