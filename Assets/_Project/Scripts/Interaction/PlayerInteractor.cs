using UnityEngine;
using Hordewood.Input;

namespace Hordewood.Interaction
{
    public class PlayerInteractor : MonoBehaviour
    {
        private PlayerControls _controls;
        private IInteractable _current;

        private void OnEnable()
        {
            _controls = PlayerControlsProvider.Instance.Controls;
        }

        private void Update()
        {
            if (_current == null) return;

            if (_controls.Player.Interact.WasPressedThisFrame())
                _current.Interact(this);
        }

        public void SetFocus(IInteractable interactable) => _current = interactable;

        public void ClearFocus(IInteractable interactable)
        {
            if (_current == interactable)
                _current = null;
        }
    }
}
