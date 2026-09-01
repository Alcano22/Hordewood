using UnityEngine;

namespace Hordewood.Player
{
    [System.Serializable]
    public struct StatModifier
    {
        public StatType type;
        public float value;
        public bool isPercentage;
    }
}
