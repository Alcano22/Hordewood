using UnityEngine;
using UnityEngine.UI;
using Hordewood.Input;

namespace Hordewood.UI
{
    public class InteractPromptUI : MonoBehaviour
    {
        [SerializeField] private float fadeSpeed = 6f;
        [SerializeField] private Image icon;
        [SerializeField] private InputIconSet iconSet;

        private float _targetAlpha;

        private void Awake() => SetAlphaImmediate(0f);

        private void OnEnable()
        {
            if (InputDeviceTracker.Instance == null) return;

            InputDeviceTracker.Instance.OnDeviceChanged += UpdateIconSprite;
            UpdateIconSprite(InputDeviceTracker.Instance.CurrentDevice);
        }

        private void OnDisable()
        {
            if (InputDeviceTracker.Instance != null)
                InputDeviceTracker.Instance.OnDeviceChanged -= UpdateIconSprite;
        }

        public void Show() => _targetAlpha = 1f;
        public void Hide() => _targetAlpha = 0f;

        private void Update()
        {
            if (Mathf.Approximately(icon.color.a, _targetAlpha)) return;

            float newAlpha = Mathf.MoveTowards(icon.color.a, _targetAlpha, fadeSpeed * Time.deltaTime);
            SetAlpha(newAlpha);
        }

        private void SetAlpha(float alpha)
        {
            Color c = icon.color;
            c.a = alpha;
            icon.color = c;
        }

        private void SetAlphaImmediate(float alpha) => SetAlpha(alpha);

        private void UpdateIconSprite(InputDeviceKind kind)
        {
            icon.sprite = iconSet.GetIcon(kind);
        }
    }
}
