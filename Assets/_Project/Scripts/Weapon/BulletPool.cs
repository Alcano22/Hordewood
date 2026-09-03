using UnityEngine;
using Hordewood.Core;

namespace Hordewood.Weapons
{
    public class BulletPool : Singleton<BulletPool>, IBulletPool
    {
        [SerializeField] private Bullet bulletPrefab;
        [SerializeField] private int prewarmCount = 64;

        private ObjectPool<Bullet> _pool;

        protected override void Awake()
        {
            base.Awake();

            _pool = new ObjectPool<Bullet>(bulletPrefab, transform, prewarmCount);
        }

        public Bullet Get(Vector3 position, Quaternion rotation)
        {
            Bullet bullet = _pool.Get(position, rotation);
            bullet.SetOwnerPool(this);
            return bullet;
        }

        public void Release(Bullet bullet) => _pool.Release(bullet);
    }
}
