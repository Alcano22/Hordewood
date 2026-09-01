using System.Collections;
using UnityEngine;

namespace Hordewood.Items
{
    [RequireComponent(typeof(SpriteRenderer))]
    public abstract class PickupBase : MonoBehaviour
    {
        [Header("Pickup Fly-In")]
        [SerializeField] private float pickupSpeed = 6f;
        [SerializeField] private float pickupAcceleration = 12f;
        [SerializeField] private float collectDistance = 0.15f;
        [SerializeField] private float minShrinkFactor = 0.3f;

        protected SpriteRenderer spriteRenderer;

        private Collider2D _collider;
        private bool _isBeingCollected;

        protected virtual void Awake()
        {
            spriteRenderer = GetComponent<SpriteRenderer>();
            _collider = GetComponent<Collider2D>();
        }

        protected abstract void OnCollected();

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (_isBeingCollected) return;
            if (!other.CompareTag("Player")) return;

            _isBeingCollected = true;
            _collider.enabled = false;
            StartCoroutine(FlyToPlayer(other.transform));
        }

        private IEnumerator FlyToPlayer(Transform player)
        {
            Vector3 startScale = transform.localScale;
            float currentSpeed = pickupSpeed;

            while (true)
            {
                Vector2 toPlayer = (Vector2)player.position - (Vector2)transform.position;
                float distance = toPlayer.magnitude;

                if (distance <= collectDistance)
                    break;

                currentSpeed += pickupAcceleration * Time.deltaTime;
                Vector2 direction = toPlayer / distance;
                transform.position += (Vector3)(direction * currentSpeed * Time.deltaTime);

                float rawShrink = Mathf.Clamp01(distance / 2f);
                float shrinkFactor = Mathf.Lerp(minShrinkFactor, 1f, rawShrink);
                transform.localScale = startScale * shrinkFactor;

                yield return null;
            }

            OnCollected();
            Destroy(gameObject);
        }
    }
}
