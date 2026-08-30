using UnityEngine;

namespace Hordewood.Weapons
{
    public enum FireMode { Semi, Auto, Burst }

    [CreateAssetMenu(fileName = "New Gun", menuName = "Hordewood/Weapons/Gun")]
    public class Gun : ScriptableObject
    {
        [Header("Info")]
        [SerializeField] private string displayName;
        [SerializeField] private Sprite icon;
        [SerializeField] private Vector2 muzzleOffset;

        [Header("Stats")]
        [SerializeField] private float damage = 5f;
        [SerializeField] private float fireRate = 500f;
        [SerializeField] private int capacity = 10;
        [SerializeField] private float reloadTime = 3f;
        [SerializeField] private float bulletSpeed = 10;
        [SerializeField] private float range = 10f;

        [Header("Behaviour")]
        [SerializeField] private FireMode fireMode = FireMode.Semi;
        [SerializeField] private int burstCount = 3;

        public string DisplayName => displayName;
        public Sprite Icon => icon;
        public Vector2 MuzzleOffset => muzzleOffset;
        public float Damage => damage;
        public float FireRate => fireRate;
        public int Capacity => capacity;
        public float ReloadTime => reloadTime;
        public float BulletSpeed => bulletSpeed;
        public float Range => range;
        public FireMode FireMode => fireMode;
        public int BurstCount => burstCount;
    }
}
