using System.Collections.Generic;
using UnityEngine;
using Hordewood.World;

namespace Hordewood.Enemies
{
    public class EnemyPathfinder : MonoBehaviour
    {
        [SerializeField] private float repathInterval = 0.5f;
        [SerializeField] private float waypointReachedDistance = 0.2f;

        private WorldGenerator _worldGenerator;
        private List<Vector3Int> _path;
        private int _waypointIndex;
        private float _repathTimer;

        private void Awake() => _repathTimer = Random.Range(0f, repathInterval);

        public void SetWorldGenerator(WorldGenerator worldGenerator) => _worldGenerator = worldGenerator;

        public void UpdatePath(Vector2 currentPos, Vector2 targetPos)
        {
            _repathTimer -= Time.deltaTime;
            if (_repathTimer > 0f) return;

            _repathTimer = repathInterval;

            Vector3Int start = _worldGenerator.WorldToCell(currentPos);
            Vector3Int goal = _worldGenerator.WorldToCell(targetPos);

            _path = Pathfinding.FindPath(_worldGenerator, start, goal);
            _waypointIndex = (_path != null && _path.Count > 1) ? 1 : 0;
        }

        public Vector2 GetMoveDirection(Vector2 currentPos)
        {
            if (_path == null || _waypointIndex >= _path.Count)
                return Vector2.zero;

            Vector2 waypoint = _worldGenerator.CellToWorld(_path[_waypointIndex]);

            if (Vector2.Distance(currentPos, waypoint) < waypointReachedDistance)
            {
                _waypointIndex++;
                if (_waypointIndex >= _path.Count)
                    return Vector2.zero;
                waypoint = _worldGenerator.CellToWorld(_path[_waypointIndex]);
            }

            return (waypoint - currentPos).normalized;
        }

        public bool IsValidLandingSpot(Vector2 worldPos)
        {
            if (_worldGenerator == null)
                return false;
            return _worldGenerator.IsGroundCell(_worldGenerator.WorldToCell(worldPos));
        }

        private void OnDrawGizmos()
        {
            if (_path == null || _worldGenerator == null) return;

            Gizmos.color = Color.cyan;

            for (int i = 0; i < _path.Count; i++)
            {
                Vector2 point = _worldGenerator.CellToWorld(_path[i]);
                Gizmos.DrawSphere(point, 0.08f);

                if (i >= _path.Count - 1) continue;

                Vector2 nextPoint = _worldGenerator.CellToWorld(_path[i + 1]);
                Gizmos.DrawLine(point, nextPoint);
            }

            if (_waypointIndex < _path.Count)
            {
                Gizmos.color = Color.red;
                Vector2 currentTarget = _worldGenerator.CellToWorld(_path[_waypointIndex]);
                Gizmos.DrawWireSphere(currentTarget, 0.15f);
            }
        }
    }
}
