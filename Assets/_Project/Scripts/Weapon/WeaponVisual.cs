using UnityEngine;

namespace Hordewood.Weapons
{
    public class WeaponVisual : MonoBehaviour
    {
        [SerializeField] private SpriteRenderer spriteRenderer;
        [SerializeField] private float orbitRadius = 0.6f;

        private Vector2 _muzzleOffset;
        private bool _isFlipped;

        private void Awake()
        {
            transform.localRotation = Quaternion.Euler(0f, 0f, 180f);
            transform.localPosition = new Vector3(orbitRadius, 0f, 0f);
        }

        public void SetGun(Gun gun)
        {
            spriteRenderer.sprite = gun.Icon;
            _muzzleOffset = gun.MuzzleOffset;
        }

        private void LateUpdate()
        {
            float worldAngle = transform.eulerAngles.z;
            if (worldAngle > 180f)
                worldAngle -= 360f;

            _isFlipped = worldAngle > 90f || worldAngle < -90f;
            spriteRenderer.flipY = _isFlipped;
        }

        public Vector2 GetMuzzlePosition()
        {
            Vector2 offset = _muzzleOffset;
            if (_isFlipped)
                offset.y = -offset.y;

            return (Vector2)transform.position + (Vector2)(transform.rotation * offset);
        }
    }
}
