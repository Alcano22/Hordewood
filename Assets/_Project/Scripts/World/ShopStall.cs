using UnityEngine;
using Hordewood.Interaction;
using Hordewood.UI;

namespace Hordewood.World
{
    public class ShopStall : MonoBehaviour, IInteractable
    {
        [SerializeField] private InteractPromptUI interactPrompt;

        public void Interact(PlayerInteractor interactor)
        {
            Debug.Log("Shop opened - UI WIP");
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
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
