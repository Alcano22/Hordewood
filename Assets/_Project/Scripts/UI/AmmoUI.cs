using UnityEngine;
using TMPro;
using Hordewood.Weapons;

namespace Hordewood.UI
{
    public class AmmoUI : MonoBehaviour
    {
        [SerializeField] private GunController gunController;
        [SerializeField] private TextMeshProUGUI ammoText;

        private void OnEnable()
        {
            gunController.OnAmmoChanged += UpdateText;
        }

        private void OnDisable()
        {
            gunController.OnAmmoChanged -= UpdateText;
        }

        private void UpdateText(int current, int capacity)
        {
            ammoText.text = $"{current} / {capacity}";
        }
    }
}
