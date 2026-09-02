using System.Text;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Hordewood.Items;
using Hordewood.Player;
using UnityEngine.Localization;
using Hordewood.Weapons;
using Hordewood.Localization;

namespace Hordewood.UI
{
    public class ShopOfferUI : MonoBehaviour
    {
        [SerializeField] private Image icon;
        [SerializeField] private Image rarityTint;
        [SerializeField, Range(0f, 1f)] private float rarityTintAlpha = 0.2f;
        [SerializeField] private TextMeshProUGUI nameText;
        [SerializeField] private TextMeshProUGUI priceText;
        [SerializeField] private TextMeshProUGUI descriptionText;
        [SerializeField] private Button buyButton;

        [Header("Modifier Colors")]
        [SerializeField] private Color positiveColor = new(0.4f, 0.9f, 0.4f);
        [SerializeField] private Color negativeColor = new(0.9f, 0.3f, 0.3f);

        [Header("Localization")]
        [SerializeField] private StatDisplayNames statNames;
        [SerializeField] private LocalizedString gunDamageLabel;
        [SerializeField] private LocalizedString gunFireRateLabel;
        [SerializeField] private LocalizedString gunCapacityLabel;

        private ShopOffer _offer;
        private System.Action<ShopOffer> _onBuy;

        private void Awake()
        {
            icon.preserveAspect = true;
        }

        public void Setup(ShopOffer offer, System.Action<ShopOffer> onBuy)
        {
            _offer = offer;
            _onBuy = onBuy;

            if (offer.IsGun)
            {
                icon.sprite = offer.Gun.Icon;
                nameText.text = offer.Gun.DisplayName;
                Color tint = offer.Gun.Rarity.GetColor();
                tint.a = rarityTintAlpha;
                rarityTint.color = tint;
                descriptionText.text = BuildGunDescription(offer.Gun);
            } else
            {
                icon.sprite = offer.PassiveItem.Icon;
                nameText.text = offer.PassiveItem.DisplayName;
                Color tint = offer.PassiveItem.Rarity.GetColor();
                tint.a = rarityTintAlpha;
                rarityTint.color = tint;
                descriptionText.text = BuildModifierDescription(offer.PassiveItem.Modifiers);
            }

            priceText.text = offer.Price.ToString();

            buyButton.onClick.RemoveAllListeners();
            buyButton.onClick.AddListener(OnBuyClicked);
        }

        private string BuildGunDescription(Gun gun)
        {
            var sb = new StringBuilder();
            sb.Append($"{LocalizationService.Instance.GetString(gunDamageLabel)}: <b>{gun.Damage:0.#}</b>\n");
            sb.Append($"{LocalizationService.Instance.GetString(gunFireRateLabel)}: <b>{gun.FireRate:0.#}</b>\n");
            sb.Append($"{LocalizationService.Instance.GetString(gunCapacityLabel)}: <b>{gun.Capacity}</b>");
            return sb.ToString();
        }

        private string BuildModifierDescription(StatModifier[] modifiers)
        {
            var sb = new StringBuilder();

            for (int i = 0; i < modifiers.Length; i++)
            {
                var mod = modifiers[i];
                string colorHex = ColorUtility.ToHtmlStringRGB(mod.IsPositive ? positiveColor : negativeColor);

                sb.Append($"<color=#{colorHex}>{mod.GetDescription(statNames)}</color>");

                if (i < modifiers.Length - 1)
                    sb.Append('\n');
            }

            return sb.ToString();
        }

        private void OnBuyClicked() => _onBuy?.Invoke(_offer);

        public void SetInteractable(bool interactable) => buyButton.interactable = interactable;
    }
}
