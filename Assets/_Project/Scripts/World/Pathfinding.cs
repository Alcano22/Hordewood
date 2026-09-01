using UnityEngine;
using System.Collections.Generic;

namespace Hordewood.World
{
    public static class Pathfinding
    {
        private static readonly Vector3Int[] Directions =
        {
            new( 1, 0, 0), new(-1,  0, 0),
            new( 0, 1, 0), new( 0, -1, 0),
            new( 1, 1, 0), new( 1, -1, 0),
            new(-1, 1, 0), new(-1, -1, 0)
        };

        public static List<Vector3Int> FindPath(WorldGenerator world, 
                                                Vector3Int start, 
                                                Vector3Int goal, 
                                                int maxNodes = 500)
        {
            var open = new MinHeap();
            var cameFrom = new Dictionary<Vector3Int, Vector3Int>();
            var gScore = new Dictionary<Vector3Int, float> { [start] = 0f };
            var closed = new HashSet<Vector3Int>();

            open.Push(start, Heuristic(start, goal));

            int explored = 0;

            while (open.Count > 0)
            {
                if (++explored > maxNodes)
                    return null;

                Vector3Int current = open.Pop();

                if (closed.Contains(current)) continue;

                if (current == goal)
                    return ReconstructPath(cameFrom, current);

                closed.Add(current);

                foreach (var dir in Directions)
                {
                    Vector3Int neighbor = current + dir;
                    if (closed.Contains(neighbor)) continue;
                    if (!world.HasGroundTile(neighbor)) continue;
                    if (world.IsBlocked(neighbor) && neighbor != goal) continue;
                    if (!world.IsFullyGroundTile(neighbor) && neighbor != goal) continue;

                    float moveCost = (dir.x != 0 && dir.y != 0) ? 1.41421356f : 1f;
                    float tentativeG = gScore[current] + moveCost;

                    if (gScore.TryGetValue(neighbor, out float existingG) && tentativeG >= existingG) continue;

                    cameFrom[neighbor] = current;
                    gScore[neighbor] = tentativeG;
                    open.Push(neighbor, tentativeG + Heuristic(neighbor, goal));
                }
            }

            return null;
        }

        private static float Heuristic(Vector3Int a, Vector3Int b) => Vector3Int.Distance(a, b);

        private static List<Vector3Int> ReconstructPath(Dictionary<Vector3Int, Vector3Int> cameFrom,
                                                        Vector3Int current)
        {
            var path = new List<Vector3Int> { current };
            while (cameFrom.TryGetValue(current, out var prev))
            {
                current = prev;
                path.Add(current);
            }
            path.Reverse();
            return path;
        }

        private class MinHeap
        {
            private readonly List<(Vector3Int cell, float priority)> _items = new();

            public int Count => _items.Count;

            public void Push(Vector3Int cell, float priority)
            {
                _items.Add((cell, priority));
                int i = _items.Count - 1;

                while (i > 0)
                {
                    int parent = (i - 1) / 2;
                    if (_items[parent].priority <= _items[i].priority) break;

                    (_items[parent], _items[i]) = (_items[i], _items[parent]);
                    i = parent;
                }
            }

            public Vector3Int Pop()
            {
                Vector3Int result = _items[0].cell;
                int last = _items.Count - 1;

                _items[0] = _items[last];
                _items.RemoveAt(last);

                int i = 0;
                while (true)
                {
                    int left = i * 2 + 1;
                    int right = i * 2 + 2;
                    int smallest = i;

                    if (left < _items.Count && _items[left].priority < _items[smallest].priority)
                        smallest = left;
                    if (right < _items.Count && _items[right].priority < _items[smallest].priority)
                        smallest = right;

                    if (smallest == i) break;

                    (_items[smallest], _items[i]) = (_items[i], _items[smallest]);
                    i = smallest;
                }

                return result;
            }
        }
    }
}
