
namespace Hordewood.Player
{
    public enum StatType
    {
        MoveSpeed,
        Damage,
        FireRate,
        MaxHealth
    }

    public static class StatTypeExtensions
    {
        public static string GetDisplayName(this StatType type) => type switch
        {
            StatType.MoveSpeed => "Move Speed",
            StatType.Damage    => "Damage",
            StatType.FireRate  => "Fire Rate",
            StatType.MaxHealth => "Max Health",
            _ => type.ToString()
        };
    }
}
