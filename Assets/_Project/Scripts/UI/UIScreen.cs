using UnityEngine;

namespace Hordewood.UI
{
    public abstract class UIScreen : MonoBehaviour
    {
        [SerializeField] private CanvasGroup canvasGroup;

        public bool IsOpen { get; private set; }

        protected virtual void Awake() => SetVisible(false);

        protected virtual void Start()
        {
            if (ScreenManager.Instance != null)
                ScreenManager.Instance.Register(this);
        }

        protected virtual void OnDestroy()
        {
            if (ScreenManager.Instance != null)
                ScreenManager.Instance.Unregister(this);
        }

        public void Open()
        {
            if (IsOpen) return;

            IsOpen = true;
            SetVisible(true);
            ScreenManager.Instance.NotifyOpened(this);
            OnOpened();
        }

        public void Close()
        {
            if (!IsOpen) return;

            IsOpen = false;
            SetVisible(false);
            ScreenManager.Instance.NotifyClosed(this);
            OnClosed();
        }

        private void SetVisible(bool visible)
        {
            canvasGroup.alpha = visible ? 1f : 0f;
            canvasGroup.interactable = visible;
            canvasGroup.blocksRaycasts = visible;
        }

        protected virtual void OnOpened() {}
        protected virtual void OnClosed() {}
    }
}
