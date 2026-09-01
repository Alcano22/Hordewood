using System.Collections.Generic;
using UnityEngine;
using Hordewood.Core;
using Hordewood.Items;
using Hordewood.Player;
using Hordewood.Weapons;

namespace Hordewood.UI
{
    public class ShopScreen : UIScreen
    {
        [SerializeField] private WaveManager waveManager;
        [SerializeField] private ShopPool shopPool;
        [SerializeField] private int offerCount = 4;

        [Header("UI")]
        [SerializeField] private ShopOfferUI offerPrefab;
        [SerializeField] private Transform offerContainer;
        [SerializeField] private GunController playerGunController;
        [SerializeField] private PlayerStats playerStats;

        private List<ShopOffer> _currentOffers = new();
        private readonly List<ShopOfferUI> _offerUIs = new();

        public event System.Action OnOffersChanged;

        protected override void Start()
        {
            base.Start();

            waveManager.OnWaveEnded += RollNewOffers;
            CurrencySystem.Instance.OnCoinsChanged += RefreshAffordability;
        }

        protected override void OnDestroy()
        {
            if (waveManager != null)
                waveManager.OnWaveEnded -= RollNewOffers;

            if (CurrencySystem.Instance != null)
                CurrencySystem.Instance.OnCoinsChanged -= RefreshAffordability;
        }

        private void RollNewOffers()
        {
            _currentOffers = shopPool.RollOffers(offerCount, playerStats);
            RebuildOfferUI();
            OnOffersChanged?.Invoke();
        }

        private void RebuildOfferUI()
        {
            foreach (var ui in _offerUIs)
                Destroy(ui.gameObject);
            _offerUIs.Clear();

            foreach (var offer in _currentOffers)
            {
                ShopOfferUI ui = Instantiate(offerPrefab, offerContainer);
                ui.Setup(offer, TryBuy);
                _offerUIs.Add(ui);
            }

            RefreshAffordability(CurrencySystem.Instance.Coins);
        }

        private void TryBuy(ShopOffer offer)
        {
            if (!CurrencySystem.Instance.TrySpend(offer.Price)) return;

            if (offer.IsGun)
                playerGunController.EquipGun(offer.Gun);
            else
                playerStats.AddPassive(offer.PassiveItem);

            RemoveOffer(offer);
        }

        private void RemoveOffer(ShopOffer offer)
        {
            int index = _currentOffers.IndexOf(offer);
            if (index < 0) return;

            _currentOffers.RemoveAt(index);
            Destroy(_offerUIs[index].gameObject);
            _offerUIs.RemoveAt(index);

            OnOffersChanged?.Invoke();
        }

        private void RefreshAffordability(int currentCoins)
        {
            for (int i = 0; i < _currentOffers.Count; i++)
                _offerUIs[i].SetInteractable(currentCoins >= _currentOffers[i].Price);
        }

        public IReadOnlyList<ShopOffer> CurrentOffers => _currentOffers;
    }
}
