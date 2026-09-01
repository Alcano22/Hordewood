using System.Collections.Generic;
using UnityEngine.InputSystem;
using Hordewood.Core;
using Hordewood.Input;

namespace Hordewood.UI
{
    public class ScreenManager : Singleton<ScreenManager>
    {
        private readonly Dictionary<System.Type, UIScreen> _screens = new();

        private UIScreen _activeScreen;
        private PlayerControls _controls;

        public event System.Action<bool> OnAnyScreenOpenChanged;

        private void OnEnable()
        {
            _controls = PlayerControlsProvider.Instance.Controls;
            _controls.UI.Cancel.performed += OnCancelPressed;
        }

        private void OnDisable()
        {
            _controls.UI.Cancel.performed -= OnCancelPressed;
        }

        private void OnCancelPressed(InputAction.CallbackContext ctx) => CloseActive();

        public void Register(UIScreen screen) => _screens[screen.GetType()] = screen;

        public void Unregister(UIScreen screen)
        {
            if (_screens.TryGetValue(screen.GetType(), out var registered) && registered == screen)
                _screens.Remove(screen.GetType());
        }

        public void Open<T>() where T : UIScreen
        {
            if (_screens.TryGetValue(typeof(T), out var screen))
                screen.Open();
        }

        public void NotifyOpened(UIScreen screen)
        {
            if (_activeScreen != null && _activeScreen != screen)
                _activeScreen.Close();

            bool wasOpen = _activeScreen != null;
            _activeScreen = screen;

            if (!wasOpen)
                OnAnyScreenOpenChanged?.Invoke(true);
        }

        public void NotifyClosed(UIScreen screen)
        {
            if (_activeScreen != screen) return;

            _activeScreen = null;
            OnAnyScreenOpenChanged?.Invoke(false);
        }

        public void CloseActive() => _activeScreen?.Close();

        public bool IsAnyScreenOpen => _activeScreen != null;
    }
}
