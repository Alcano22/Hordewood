using UnityEngine;
using Hordewood.Core;
using Hordewood.Interaction;
using Hordewood.UI;

namespace Hordewood.Weapons
{
    [RequireComponent(typeof(CircleCollider2D))]
    public class WeaponPickup : MonoBehaviour, IInteractable
    {
        [SerializeField] private Gun gun;
        [SerializeField] private SpriteRenderer spriteRenderer;
        [SerializeField] private InteractPromptUI interactPrompt;

        private CircleCollider2D _collider;

        private void Awake()
        {
            _collider = GetComponent<CircleCollider2D>();

            if (gun != null)
                RefreshVisual();
        }

        public void Init(Gun newGun)
        {
            gun = newGun;
            RefreshVisual();
        }

        private void RefreshVisual()
        {
            spriteRenderer.sprite = gun.Icon;
            spriteRenderer.transform.localScale = new Vector3(gun.VisualScale, gun.VisualScale, 1f);

            Bounds spriteBounds = gun.Icon.bounds;
            _collider.offset = new Vector2(spriteBounds.center.x, spriteBounds.center.y) * gun.VisualScale;
        }

        public void Interact(PlayerInteractor interactor)
        {
            if (!interactor.TryGetComponent<GunController>(out var gunController)) return;

            gunController.EquipGun(gun);
            interactPrompt.Hide();
            Destroy(gameObject);
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
