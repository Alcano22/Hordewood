using UnityEngine;
using Hordewood.UI;
using Hordewood.Interaction;

namespace Hordewood.World
{
    [RequireComponent(typeof(Animator))]
    public class Campfire : MonoBehaviour, IInteractable
    {
        [SerializeField] private InteractPromptUI interactPrompt;

        private Animator _animator;
        private bool _isLit;

        private void Awake() => _animator = GetComponent<Animator>();
        private void Start() => _animator.Play("Campfire_Unlit");

        public void Interact(PlayerInteractor interactor)
        {
            if (_isLit) return;

            _isLit = true;
            _animator.Play("Campfire_Lit");
            interactPrompt.Hide();
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (_isLit) return;
            if (!other.TryGetComponent<PlayerInteractor>(out var interactor)) return;

            interactor.SetFocus(this);
            interactPrompt.Show();
        }

        private void OnTriggerExit2D(Collider2D other)
        {
            if (!other.TryGetComponent<PlayerInteractor>(out var interactor)) return;

            interactor.ClearFocus(this);
            interactPrompt.Hide();
        }
    }
}
