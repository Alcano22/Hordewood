using UnityEngine;
using Hordewood.UI;
using Hordewood.Interaction;
using UnityEngine.Rendering.Universal;
using Hordewood.Player;

namespace Hordewood.World
{
    [RequireComponent(typeof(Animator))]
    public class Campfire : MonoBehaviour, IInteractable
    {
        [SerializeField] private InteractPromptUI interactPrompt;
        [SerializeField] private Light2D fireLight;

        [Header("Healing")]
        [SerializeField] private float healRadius = 3f;
        [SerializeField] private float healPerSecond = 2f;

        private Animator _animator;
        private bool _isLit;

        private Transform _playerTransform;
        private PlayerHealth _playerHealth;

        private void Awake()
        {
            _animator = GetComponent<Animator>();
            fireLight.gameObject.SetActive(false);

            fireLight.pointLightOuterRadius = healRadius;
            fireLight.pointLightInnerRadius = healRadius - 0.5f;
        }

        private void Start()
        {
            _animator.Play($"Campfire_Unlit");

            var player = GameObject.FindGameObjectWithTag("Player");
            if (player == null) return;

            _playerTransform = player.transform;
            _playerHealth = player.GetComponent<PlayerHealth>();
        }

        private void Update()
        {
            if (!_isLit || _playerTransform == null || _playerHealth == null) return;

            float distance = Vector2.Distance(transform.position, _playerTransform.position);
            if (distance <= healRadius)
                _playerHealth.Heal(healPerSecond * Time.deltaTime);
        }

        public void Interact(PlayerInteractor interactor)
        {
            if (_isLit) return;

            _isLit = true;
            _animator.Play("Campfire_Lit");
            interactPrompt.Hide();
            fireLight.gameObject.SetActive(true);
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
