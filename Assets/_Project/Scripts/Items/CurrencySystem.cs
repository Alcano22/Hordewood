using Hordewood.Core;
using UnityEngine;

namespace Hordewood.Items
{
    public class CurrencySystem : Singleton<CurrencySystem>
    {
        public int Coins { get; private set; }

        public event System.Action<int> OnCoinsChanged;
        public event System.Action<int> OnCoinsAdded;

        public void Add(int amount = 1)
        {
            Coins += amount;
            OnCoinsChanged?.Invoke(Coins);
            OnCoinsAdded?.Invoke(amount);
        }

        public bool TrySpend(int amount)
        {
            if (Coins < amount)
                return false;

            Coins -= amount;
            OnCoinsChanged?.Invoke(Coins);
            return true;
        }
    }
}
