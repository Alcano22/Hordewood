using System.Collections;
using UnityEngine;

namespace Hordewood.Core
{
    public class CameraShake : MonoBehaviour
    {
        [SerializeField] private float maxOffset = 0.3f;
        [SerializeField] private float maxAngle = 3f;
        [SerializeField] private float frequency = 25f;
        [SerializeField] private float traumaDecay = 1.5f;

        private Vector3 _originalLocalPosition;
        private float _trauma;
        private float _seedX;
        private float _seedY;
        private float _seedAngle;

        private void Awake()
        {
            _originalLocalPosition = transform.localPosition;
            _seedX = Random.Range(0f, 100f);
            _seedY = Random.Range(0f, 100f);
            _seedAngle = Random.Range(0f, 100f);
        }

        public void AddTrauma(float amount)
        {
            _trauma = Mathf.Clamp01(_trauma + amount);
        }

        private void Update()
        {
            if (_trauma <= 0f)
            {
                if (transform.localPosition != _originalLocalPosition)
                {
                    transform.localPosition = _originalLocalPosition;
                    transform.localRotation = Quaternion.identity;
                }
                return;
            }

            _trauma = Mathf.Max(0f, _trauma - traumaDecay * Time.deltaTime);

            float shake = _trauma * _trauma;
            float time = Time.time * frequency;

            float offsetX = (Mathf.PerlinNoise(_seedX, time) * 2f - 1f) * maxOffset * shake;
            float offsetY = (Mathf.PerlinNoise(_seedY, time) * 2f - 1f) * maxOffset * shake;
            float angle = (Mathf.PerlinNoise(_seedAngle, time) * 2f - 1f) * maxAngle * shake;

            transform.localPosition = _originalLocalPosition + new Vector3(offsetX, offsetY, 0f);
            transform.localRotation = Quaternion.Euler(0f, 0f, angle);
        }
    }
}
