using System.Collections;
using UnityEngine;
using UnityEngine.Localization;
using UnityEngine.Localization.Settings;
using Hordewood.Core;

namespace Hordewood.Localization
{
    public class LocalizationService : Singleton<LocalizationService>
    {
        [SerializeField] private Locale startupLocale;

        public event System.Action OnLocaleChanged;

        private bool _isReady;

        protected override void Awake()
        {
            base.Awake();

            StartCoroutine(InitializeRoutine());
        }

        private IEnumerator InitializeRoutine()
        {
            yield return LocalizationSettings.InitializationOperation;

            if (startupLocale)
                LocalizationSettings.SelectedLocale = startupLocale;

            _isReady = true;
        }

        public void SetLocale(Locale locale)
        {
            LocalizationSettings.SelectedLocale = locale;
            OnLocaleChanged?.Invoke();
        }

        public string GetString(LocalizedString localizedString, params object[] arguments)
        {
            if (!_isReady || localizedString == null || localizedString.IsEmpty)
                return string.Empty;

            if (arguments != null && arguments.Length > 0)
                localizedString.Arguments = arguments;

            return localizedString.GetLocalizedString();
        }
    }
}
