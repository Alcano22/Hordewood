
namespace Hordewood.Combat
{
    public interface IHealthSource
    {
        float CurrentHealth { get; }
        float MaxHealth { get; }
        event System.Action<float, float> OnHealthChanged;
    }
}
