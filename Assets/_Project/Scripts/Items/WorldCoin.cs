using UnityEngine;

namespace Hordewood.Items
{
    public class WorldCoin : PickupBase
    {
        protected override void OnCollected()
        {
            CurrencySystem.Instance.Add(1);
        }
    }
}
