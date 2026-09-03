using Hordewood.Core;
using Hordewood.UI;

namespace Hordewood.Input
{
    public class PlayerControlsProvider : Singleton<PlayerControlsProvider>
    {
        public PlayerControls Controls { get; private set; }

        protected override void Awake()
        {
            base.Awake();

            Controls = new PlayerControls();
            Controls.Player.Enable();
            Controls.UI.Enable();
        }

        private void Start()
        {
            ScreenManager.Instance.OnAnyScreenOpenChanged += SetPlayerMapEnabled;
        }

        protected override void OnDestroy()
        {
            base.OnDestroy();

            if (ScreenManager.Instance != null)
                ScreenManager.Instance.OnAnyScreenOpenChanged -= SetPlayerMapEnabled;

            Controls?.Player.Disable();
            Controls?.UI.Disable();
        }

        private void SetPlayerMapEnabled(bool anyScreenOpen)
        {
            if (anyScreenOpen)
                Controls.Player.Disable();
            else
                Controls.Player.Enable();
        }
    }
}
