using UnityEngine;
using TMPro;
using Hordewood.Items;

namespace Hordewood.UI
{
    public class CoinsUI : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI coinsText;

        private void Start()
        {
            if (CurrencySystem.Instance != null)
                CurrencySystem.Instance.OnCoinsChanged += UpdateText;

            UpdateText(CurrencySystem.Instance.Coins);
        }

        private void OnDestroy()
        {
            if (CurrencySystem.Instance != null)
                CurrencySystem.Instance.OnCoinsChanged -= UpdateText;
        }

        private void UpdateText(int coins) => coinsText.text = coins.ToString();
    }
}
