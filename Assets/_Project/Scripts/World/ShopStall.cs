using UnityEngine;
using Hordewood.Interaction;
using Hordewood.UI;
using Hordewood.Core;

namespace Hordewood.World
{
    public class ShopStall : StructureBase, IInteractable
    {
        [SerializeField] private InteractPromptUI interactPrompt;

        private bool _playerInRange;

        private void Start()
        {
            GameManager.Instance.OnStateChanged += OnGameStateChanged;
        }

        private void OnDestroy()
        {
            if (GameManager.Instance != null)
                GameManager.Instance.OnStateChanged -= OnGameStateChanged;
        }

        private void OnGameStateChanged(GameState newState) => UpdatePromptVisibility();

        public void Interact(PlayerInteractor interactor)
        {
            if (GameManager.Instance.CurrentState == GameState.WaveBreak)
                ScreenManager.Instance.Open<ShopScreen>();
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (!other.TryGetComponent<PlayerInteractor>(out var interactor)) return;

            _playerInRange = true;
            interactor.SetFocus(this);
            UpdatePromptVisibility();
        }

        private void OnTriggerExit2D(Collider2D other)
        {
            if (!other.TryGetComponent<PlayerInteractor>(out var interactor)) return;

            _playerInRange = false;
            interactor.ClearFocus(this);
            interactPrompt.Hide();
        }

        private void UpdatePromptVisibility()
        {
            if (_playerInRange && GameManager.Instance.CurrentState == GameState.WaveBreak)
                interactPrompt.Show();
            else
                interactPrompt.Hide();
        }
    }
}
