using UnityEngine;
using UnityEngine.UI;
using Hordewood.Combat;

namespace Hordewood.UI
{
    public class HealthBarUI : MonoBehaviour
    {
        [SerializeField] private Slider slider;
        [SerializeField] private GameObject barContainer;
        [SerializeField] private bool hideWhenFull;

        private IHealthSource _healthSource;

        public void SetHealthSource(IHealthSource healthSource)
        {
            if (_healthSource != null)
                _healthSource.OnHealthChanged -= UpdateBar;

            _healthSource = healthSource;
            _healthSource.OnHealthChanged += UpdateBar;

            UpdateBar(_healthSource.CurrentHealth, _healthSource.MaxHealth);
        }

        private void OnEnable()
        {
            if (_healthSource == null) return;

            _healthSource.OnHealthChanged += UpdateBar;
            UpdateBar(_healthSource.CurrentHealth, _healthSource.MaxHealth);
        }

        private void OnDisable()
        {
            if (_healthSource != null)
                _healthSource.OnHealthChanged -= UpdateBar;
        }

        private void UpdateBar(float current, float max)
        {
            slider.value = current / max;

            if (hideWhenFull && barContainer != null)
                barContainer.SetActive(current < max);
        }
    }
}
