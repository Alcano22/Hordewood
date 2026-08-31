using System.Collections;
using UnityEngine;

namespace Hordewood.Core
{
    public class SpawnPopEffect : MonoBehaviour
    {
        [SerializeField] private float jumpHeight = 0.5f;
        [SerializeField] private float duration = 0.35f;
        [SerializeField] private float minScatterDistance = 0.3f;
        [SerializeField] private float maxScatterDistance = 1f;

        private void Start()
        {
            Vector2 randomDirection = Random.insideUnitCircle.normalized;
            float scatterDistance = Random.Range(minScatterDistance, maxScatterDistance);
            Vector2 targetPos = (Vector2)transform.position + randomDirection * scatterDistance;

            var collider = GetComponent<Collider2D>();
            if (collider != null)
                collider.enabled = false;

            StartCoroutine(PopRoutine(transform.position, targetPos, collider));
        }

        private IEnumerator PopRoutine(Vector2 startPos, Vector2 targetPos, Collider2D collider)
        {
            float elapsed = 0f;

            while (elapsed < duration)
            {
                elapsed += Time.deltaTime;
                float t = Mathf.Clamp01(elapsed / duration);

                Vector2 groundPos = Vector2.Lerp(startPos, targetPos, t);
                float arc = Mathf.Sin(t * Mathf.PI) * jumpHeight;

                transform.position = groundPos + Vector2.up * arc;
                yield return null;
            }

            transform.position = targetPos;

            if (collider != null)
                collider.enabled = true;
        }
    }
}
