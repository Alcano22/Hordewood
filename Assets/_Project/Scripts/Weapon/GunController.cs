using Hordewood.Input;
using System;
using System.Security.Claims;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Hordewood.Weapons
{
    public class GunController : MonoBehaviour
    {
        [SerializeField] private Gun gun;
        [SerializeField] private Transform firePoint;
        [SerializeField] private Bullet bulletPrefab;
        [SerializeField] private WeaponVisual weaponVisual;
        [SerializeField] private float aimStickDeadzone = 0.5f;

        [Header("Aim Assist (Gamepad only)")]
        [SerializeField] private float aimAssistAngle = 30f;
        [SerializeField] private float aimAssistStrength = 0.35f;
        [SerializeField] private LayerMask enemyLayer;

        private PlayerControls _controls;
        private Camera _mainCamera;

        private Vector2 _aimInput;

        private enum AimSource { Mouse, Gamepad }
        private AimSource _aimSource = AimSource.Mouse;

        private float _fireTimer;
        private int _currentAmmo;
        private bool _isReloading;
        private float _reloadTimer;

        public event Action<int, int> OnAmmoChanged;

        private void OnEnable()
        {
            _controls.Player.Enable();
            _controls.Player.Aim.performed += OnAim;
            _controls.Player.Aim.canceled += OnAim;
        }

        private void OnDisable()
        {
            _controls.Player.Aim.performed -= OnAim;
            _controls.Player.Aim.canceled -= OnAim;
            _controls.Player.Disable();
        }

        private void Awake()
        {
            _controls = new PlayerControls();
            _mainCamera = Camera.main;
            _currentAmmo = gun.Capacity;
            weaponVisual.SetGun(gun);
        }

        private void Start()
        {
            OnAmmoChanged?.Invoke(_currentAmmo, gun.Capacity);
        }

        private void Update()
        {
            UpdateAim();

            if (_isReloading)
            {
                _reloadTimer -= Time.deltaTime;
                if (_reloadTimer <= 0f)
                    FinishReload();
                return;
            }

            if (_controls.Player.Reload.WasPressedThisFrame() && _currentAmmo < gun.Capacity)
            {
                StartReload();
                return;
            }

            _fireTimer -= Time.deltaTime;

            if (_currentAmmo <= 0)
            {
                StartReload();
                return;
            }

            bool wantsToFire = gun.FireMode == FireMode.Auto
                             ? _controls.Player.Fire.IsPressed()
                             : _controls.Player.Fire.WasPressedThisFrame();

            if (wantsToFire && _fireTimer <= 0f)
            {
                Fire();
                _fireTimer = 60f / gun.FireRate;
            }
        }

        private void OnAim(InputAction.CallbackContext ctx)
        {
            Vector2 rawInput = ctx.ReadValue<Vector2>();
            _aimInput = rawInput.magnitude > aimStickDeadzone ? rawInput : Vector2.zero;

            if (_aimInput.sqrMagnitude > 0.01f)
                _aimSource = AimSource.Gamepad;
        }

        private void UpdateAim()
        {
            if (Mouse.current != null && Mouse.current.delta.ReadValue().sqrMagnitude > 0.01f)
                _aimSource = AimSource.Mouse;

            Vector2 direction;
            if (_aimSource == AimSource.Mouse)
            {
                Vector2 mouseWorldPos = _mainCamera.ScreenToWorldPoint(Mouse.current.position.ReadValue());
                direction = (mouseWorldPos - (Vector2)firePoint.position).normalized;
            } else
            {
                direction = _aimInput.sqrMagnitude > 0.01f
                          ? ApplyAimAssist(_aimInput.normalized)
                          : (Vector2)firePoint.right;
            }
            firePoint.right = direction;
        }

        private Vector2 ApplyAimAssist(Vector2 rawDirection)
        {
            Collider2D[] nearbyEnemies = Physics2D.OverlapCircleAll(firePoint.position, gun.Range, enemyLayer);

            Transform bestTarget = null;
            float bestAngle = aimAssistAngle;

            foreach (var col in nearbyEnemies)
            {
                Vector2 toEnemy = (Vector2)col.transform.position - (Vector2)firePoint.position;
                float angle = Vector2.Angle(rawDirection, toEnemy);
                if (angle >= bestAngle) continue;

                bestAngle = angle;
                bestTarget = col.transform;
            }

            if (bestTarget == null)
                return rawDirection;

            Vector2 toTarget = ((Vector2)bestTarget.position - (Vector2)firePoint.position).normalized;
            return Vector2.Lerp(rawDirection, toTarget, aimAssistStrength).normalized;
        }

        private void Fire()
        {
            _currentAmmo--;
            OnAmmoChanged?.Invoke(_currentAmmo, gun.Capacity);

            Vector2 muzzlePos = weaponVisual.GetMuzzlePosition();
            Bullet bullet = Instantiate(bulletPrefab, muzzlePos, firePoint.rotation);
            bullet.Init(firePoint.right, gun.BulletSpeed, gun.Damage, gun.Range);
        }

        private void StartReload()
        {
            _isReloading = true;
            _reloadTimer = gun.ReloadTime;
        }

        private void FinishReload()
        {
            _isReloading = false;
            _currentAmmo = gun.Capacity;
            OnAmmoChanged?.Invoke(_currentAmmo, gun.Capacity);
        }

        public bool IsReloading => _isReloading;
        public float ReloadProgress => _isReloading ? 1f - (_reloadTimer / gun.ReloadTime) : 0f;
    }
}
