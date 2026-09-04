using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Localization;
using UnityEngine.Localization.Settings;
using TMPro;
using Hordewood.Localization;

namespace Hordewood.UI
{
    public class LanguageDropdown : MonoBehaviour
    {
        [SerializeField] private TMP_Dropdown dropdown;

        private List<Locale> _locales;

        private void Start()
        {
            StartCoroutine(SetupRoutine());
        }

        private IEnumerator SetupRoutine()
        {
            yield return LocalizationSettings.InitializationOperation;

            _locales = LocalizationSettings.AvailableLocales.Locales;

            var options = new List<string>();
            foreach (var locale in _locales)
                options.Add(locale.LocaleName);

            dropdown.ClearOptions();
            dropdown.AddOptions(options);

            int currentIndex = _locales.IndexOf(LocalizationSettings.SelectedLocale);
            dropdown.SetValueWithoutNotify(currentIndex >= 0 ? currentIndex : 0);

            dropdown.onValueChanged.AddListener(OnDropdownChanged);
        }

        private void OnDropdownChanged(int index)
        {
            LocalizationService.Instance.SetLocale(_locales[index]);
        }
    }
}
