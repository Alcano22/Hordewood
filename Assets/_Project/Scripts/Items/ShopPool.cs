using System.Collections.Generic;
using UnityEngine;
using Hordewood.Player;
using Hordewood.Weapons;

namespace Hordewood.Items
{
    [CreateAssetMenu(fileName = "New Shop Pool", menuName = "Hordewood/Items/ShopPool")]
    public class ShopPool : ScriptableObject
    {
        [SerializeField] private Gun[] availableGuns;
        [SerializeField] private PassiveItem[] availablePassives;
        [SerializeField] private RarityConfig rarityConfig;

        public List<ShopOffer> RollOffers(int count, PlayerStats playerStats)
        {
            var offers = new List<ShopOffer>(count);
            var usedGuns = new HashSet<Gun>();
            var usedPassives = new HashSet<PassiveItem>();

            int guard = 0;
            while (offers.Count < count && guard < count * 20)
            {
                guard++;

                ShopOffer? offer = RollOffer(playerStats, usedGuns, usedPassives);
                if (offer == null) continue;

                offers.Add(offer.Value);

                if (offer.Value.IsGun)
                    usedGuns.Add(offer.Value.Gun);
                else
                    usedPassives.Add(offer.Value.PassiveItem);
            }

            return offers;
        }

        private ShopOffer? RollOffer(PlayerStats playerStats, 
                                     HashSet<Gun> usedGuns, 
                                     HashSet<PassiveItem> usedPassives)
        {
            var validGuns = new List<Gun>();
            foreach (var gun in availableGuns)
            {
                if (!usedGuns.Contains(gun))
                    validGuns.Add(gun);
            }

            var validPassives = new List<PassiveItem>();
            foreach (var item in availablePassives)
            {
                if (usedPassives.Contains(item)) continue;
                if (item.IsUnique && playerStats.HasPassive(item)) continue;

                validPassives.Add(item);
            }

            bool canRollGun = validGuns.Count > 0;
            bool canRollPassive = validPassives.Count > 0;

            if (!canRollGun && !canRollPassive)
                return null;

            bool rollGun = canRollGun && (!canRollPassive || Random.value < 0.5f);
            if (rollGun)
            {
                Gun gun = RollWeighted(validGuns.ToArray(), g => g.Rarity);
                return new ShopOffer(gun, rarityConfig.GetBasePrice(gun.Rarity));
            } else
            {
                PassiveItem item = RollWeighted(validPassives.ToArray(), p => p.Rarity);
                return new ShopOffer(item, rarityConfig.GetBasePrice(item.Rarity));
            }
        }

        private T RollWeighted<T>(T[] pool, System.Func<T, ItemRarity> getRarity)
        {
            float totalWeight = 0f;
            foreach (var entry in pool)
                totalWeight += rarityConfig.GetWeight(getRarity(entry));

            float roll = Random.Range(0f, totalWeight);
            float cumulative = 0f;

            foreach (var entry in pool)
            {
                cumulative += rarityConfig.GetWeight(getRarity(entry));
                if (roll <= cumulative)
                    return entry;
            }

            return pool[pool.Length - 1];
        }
    }
}
