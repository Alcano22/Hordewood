using System.Collections.Generic;
using UnityEngine;

namespace Hordewood.Core
{
    public class ObjectPool<T> where T : Component
    {
        private readonly T _prefab;
        private readonly Transform _parent;
        private readonly Stack<T> _inactive = new();

        public ObjectPool(T prefab, Transform parent, int prewarmCount = 0)
        {
            _prefab = prefab;
            _parent = parent;

            for (int i = 0; i < prewarmCount; i++)
                _inactive.Push(CreateInstance());
        }

        private T CreateInstance()
        {
            T instance = Object.Instantiate(_prefab, _parent);
            instance.gameObject.SetActive(false);
            return instance;
        }

        public T Get(Vector3 position, Quaternion rotation)
        {
            T instance = _inactive.Count > 0 ? _inactive.Pop() : CreateInstance();
            instance.transform.SetPositionAndRotation(position, rotation);
            instance.gameObject.SetActive(true);

            if (instance is IPoolable poolable)
                poolable.OnSpawned();

            return instance;
        }

        public void Release(T instance)
        {
            if (instance is IPoolable poolable)
                poolable.OnDespawned();

            instance.gameObject.SetActive(false);
            instance.transform.SetParent(_parent);
            _inactive.Push(instance);
        }
    }
}
