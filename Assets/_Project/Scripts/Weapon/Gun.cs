using System;
using UnityEngine;

namespace Hordewood.Weapons
{
    public enum FireMode { Semi, Auto, Burst, Spread }

    [CreateAssetMenu(fileName = "New Gun", menuName = "Hordewood/Weapons/Gun")]
    public class Gun : ScriptableObject
    {
        [SerializeField] private string displayName;
        [SerializeField] private Sprite icon;
        [SerializeField] private Vector2 muzzleOffset;
        [SerializeField] private float visualScale = 1f;

        [SerializeField] private float damage = 5f;
        [SerializeField] private float fireRate = 500f;
        [SerializeField] private int capacity = 10;
        [SerializeField] private float reloadTime = 3f;
        [SerializeField] private float bulletSpeed = 10;
        [SerializeField] private float range = 10f;

        [SerializeField] private FireMode fireMode = FireMode.Semi;
        [SerializeField] private int burstCount = 3;
        [SerializeField] private float burstShotDelay = 0.08f;
        [SerializeField] private int pelletCount = 6;
        [SerializeField] private float spreadAngle = 25f;

        [SerializeField] private AudioClip fireSound;

        public string DisplayName => displayName;
        public Sprite Icon => icon;
        public Vector2 MuzzleOffset => muzzleOffset;
        public float VisualScale => visualScale;
        public float Damage => damage;
        public float FireRate => fireRate;
        public float SecondsBetweenShots => 60f / fireRate;
        public int Capacity => capacity;
        public float ReloadTime => reloadTime;
        public float BulletSpeed => bulletSpeed;
        public float Range => range;
        public FireMode FireMode => fireMode;
        public int BurstCount => burstCount;
        public float BurstShotDelay => burstShotDelay;
        public int PelletCount => pelletCount;
        public float SpreadAngle => spreadAngle;
        public AudioClip FireSound => fireSound;
    }
}
