using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace Hordewood.UI
{
    public class PickupPopupUI : MonoBehaviour
    {
        [SerializeField] private Image icon;
        [SerializeField] private TextMeshProUGUI amountText;
        [SerializeField] private float lifetime = 2f;
        [SerializeField] private float fadeDuration = 0.5f;

        private CanvasGroup _canvasGroup;
        private Coroutine _lifeRoutine;
        private int _currentAmount;
        private string _displayName;

        public void Setup(Sprite sprite, int amount, string displayName)
        {
            icon.sprite = sprite;
            _currentAmount = amount;
            _displayName = displayName;
            _canvasGroup = gameObject.AddComponent<CanvasGroup>();
            UpdateText();
            RestartLifetime();
        }

        public void AddAmount(int amount)
        {
            _currentAmount += amount;
            UpdateText();
            RestartLifetime();
        }

        private void UpdateText() => amountText.text = $"+{_currentAmount} {_displayName}";

        private void RestartLifetime()
        {
            if (_lifeRoutine != null)
                StopCoroutine(_lifeRoutine);

            _canvasGroup.alpha = 1f;
            _lifeRoutine = StartCoroutine(LifeRoutine());
        }

        private IEnumerator LifeRoutine()
        {
            yield return new WaitForSeconds(lifetime);

            float elapsed = 0f;
            while (elapsed < fadeDuration)
            {
                elapsed += Time.deltaTime;
                _canvasGroup.alpha = 1f - Mathf.Clamp01(elapsed / fadeDuration);
                yield return null;
            }

            Destroy(gameObject);
        }
    }
}
