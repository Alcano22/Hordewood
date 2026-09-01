using UnityEngine;

namespace Hordewood.UI
{
    public abstract class UIScreen : MonoBehaviour
    {
        [SerializeField] private GameObject root;

        public bool IsOpen { get; private set; }

        protected virtual void Awake() => root.SetActive(false);

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
            root.SetActive(true);
            ScreenManager.Instance.NotifyOpened(this);
            OnOpened();
        }

        public void Close()
        {
            if (!IsOpen) return;

            IsOpen = false;
            root.SetActive(false);
            ScreenManager.Instance.NotifyClosed(this);
            OnClosed();
        }

        protected virtual void OnOpened() {}
        protected virtual void OnClosed() {}
    }
}
