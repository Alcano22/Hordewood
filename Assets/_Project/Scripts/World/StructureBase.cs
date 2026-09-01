using UnityEngine;

namespace Hordewood.World
{
    public abstract class StructureBase : MonoBehaviour
    {
        [SerializeField] private BoxCollider2D clearanceCollider;

        protected virtual void Awake()
        {
            if (clearanceCollider != null)
                clearanceCollider.enabled = false;
        }

        public Bounds ClearanceBounds => new(clearanceCollider.offset, clearanceCollider.size);
    }
}
