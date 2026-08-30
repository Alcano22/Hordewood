using UnityEngine;
using UnityEngine.UI;
using Hordewood.Weapons;

namespace Hordewood.UI
{
    public class ReloadBarUI : MonoBehaviour
    {
        [SerializeField] private GunController gunController;
        [SerializeField] private Slider slider;
        [SerializeField] private GameObject barContainer;

        private void Update()
        {
            bool isReloading = gunController.IsReloading;

            barContainer.SetActive(isReloading);

            if (isReloading)
                slider.value = gunController.ReloadProgress;
        }
    }
}
