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
            var openSet = new List<Vector3Int> { start };
            var cameFrom = new Dictionary<Vector3Int, Vector3Int>();
            var gScore = new Dictionary<Vector3Int, float> { [start] = 0f };
            var fScore = new Dictionary<Vector3Int, float> { [start] = Heuristic(start, goal) };

            int explored = 0;

            while (openSet.Count > 0)
            {
                if (++explored > maxNodes)
                    return null;

                Vector3Int current = GetLowestFScore(openSet, fScore);
                if (current == goal)
                    return ReconstructPath(cameFrom, current);

                openSet.Remove(current);

                foreach (var dir in Directions)
                {
                    Vector3Int neighbor = current + dir;
                    if (!world.IsFullyGroundCell(neighbor) && neighbor != goal) continue;

                    float moveCost = (dir.x != 0 && dir.y != 0) ? 1.41421356f : 1f;
                    float tentativeG = gScore[current] + moveCost;

                    if (gScore.TryGetValue(neighbor, out float existingG) && tentativeG >= existingG) continue;

                    cameFrom[neighbor] = current;
                    gScore[neighbor] = tentativeG;
                    fScore[neighbor] = tentativeG + Heuristic(neighbor, goal);

                    if (!openSet.Contains(neighbor))
                        openSet.Add(neighbor);
                }
            }

            return null;
        }

        private static Vector3Int GetLowestFScore(List<Vector3Int> openSet, 
                                                  Dictionary<Vector3Int, float> fScore)
        {
            Vector3Int best = openSet[0];
            float bestScore = fScore.TryGetValue(best, out var s) ? s : float.MaxValue;

            foreach (var node in openSet)
            {
                float score = fScore.TryGetValue(node, out var sc) ? sc : float.MaxValue;
                if (score >= bestScore) continue;

                best = node;
                bestScore = score;
            }

            return best;
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
    }
}
