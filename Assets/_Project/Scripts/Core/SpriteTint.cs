using System.Collections;
using UnityEngine;

namespace Hordewood.Core
{
    [RequireComponent(typeof(SpriteRenderer))]
    public class SpriteTint : MonoBehaviour
    {
        [SerializeField] private Color defaultFlashColor = Color.white;
        [SerializeField] private float defaultFlashDuration = 0.08f;

        private SpriteRenderer _spriteRenderer;
        private Color _originalColor;
        private Coroutine _flashRoutine;
        private bool _isToggled;

        private void Awake()
        {
            _spriteRenderer = GetComponent<SpriteRenderer>();
            _originalColor = _spriteRenderer.color;
        }

        public void Flash() => Flash(defaultFlashColor, defaultFlashDuration);

        public void Flash(Color color, float duration)
        {
            if (_flashRoutine != null)
                StopCoroutine(_flashRoutine);

            _flashRoutine = StartCoroutine(FlashRoutine(color, duration));
        }

        private IEnumerator FlashRoutine(Color color, float duration)
        {
            _spriteRenderer.color = color;
            yield return new WaitForSeconds(duration);
            _flashRoutine = null;
            _spriteRenderer.color = _isToggled ? _spriteRenderer.color : _originalColor;
        }

        public void SetTint(Color color, bool active)
        {
            _isToggled = active;
            _spriteRenderer.color = active ? color : _originalColor;
        }
    }
}
