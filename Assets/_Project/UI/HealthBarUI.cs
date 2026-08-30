using UnityEngine;
using UnityEngine.UI;
using Hordewood.Player;

namespace Hordewood.UI
{
    public class HealthBarUI : MonoBehaviour
    {
        [SerializeField] private PlayerHealth playerHealth;
        [SerializeField] private Slider slider;

        private void OnEnable()
        {
            playerHealth.OnHealthChanged += UpdateBar;
        }

        private void OnDisable()
        {
            playerHealth.OnHealthChanged -= UpdateBar;
        }

        private void Start()
        {
            UpdateBar(playerHealth.CurrentHealth, playerHealth.MaxHealth);
        }

        private void UpdateBar(float current, float max)
        {
            slider.value = current / max;
        }
    }
}
