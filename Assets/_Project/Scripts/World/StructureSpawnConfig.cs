using UnityEngine;

namespace Hordewood.World
{
    [CreateAssetMenu(fileName = "New Structure Spawn Config", menuName = "Hordewood/World/StructureSpawnConfig")]
    public class StructureSpawnConfig : ScriptableObject
    {
        [SerializeField] private GameObject prefab;
        [SerializeField] private int count = 5;
        [SerializeField] private float minDistanceBetween = 15f;

        public GameObject Prefab => prefab;
        public int Count => count;
        public float MinDistanceBetween => minDistanceBetween;
    }
}
