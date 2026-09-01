using Hordewood.Weapons;

namespace Hordewood.Items
{
    public readonly struct ShopOffer
    {
        public readonly Gun Gun;
        public readonly PassiveItem PassiveItem;
        public readonly int Price;

        public bool IsGun => Gun != null;

        public ShopOffer(Gun gun, int price)
        {
            Gun = gun;
            PassiveItem = null;
            Price = price;
        }

        public ShopOffer(PassiveItem passiveItem, int price)
        {
            Gun = null;
            PassiveItem = passiveItem;
            Price = price;
        }
    }
}
