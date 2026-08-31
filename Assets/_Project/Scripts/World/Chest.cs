using UnityEngine;
using Hordewood.Interaction;
using Hordewood.UI;
using Hordewood.Weapons;

namespace Hordewood.World
{
    [RequireComponent(typeof(SpriteRenderer))]
    public class Chest : MonoBehaviour, IInteractable
    {
        [SerializeField] private Sprite closedSprite;
        [SerializeField] private Sprite openSprite;
        [SerializeField] private Gun[] possibleGuns;
        [SerializeField] private WeaponPickup weaponPickupPrefab;
        [SerializeField] private InteractPromptUI interactPrompt;

        private SpriteRenderer _spriteRenderer;
        private bool _isOpened;

        private void Awake()
        {
            _spriteRenderer = GetComponent<SpriteRenderer>();
            _spriteRenderer.sprite = closedSprite;
        }

        public void Interact(PlayerInteractor interactor)
        {
            if (_isOpened) return;

            _isOpened = true;
            _spriteRenderer.sprite = openSprite;
            interactPrompt.Hide();

            Gun rewardGun = possibleGuns[Random.Range(0, possibleGuns.Length)];
            WeaponPickup pickup = Instantiate(weaponPickupPrefab, transform.position, Quaternion.identity);
            pickup.Init(rewardGun);
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (_isOpened) return;
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
