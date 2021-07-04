using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CircularPath : MonoBehaviour, IPathGenerator
{
    public List<Vector2Int> Waypoints;

    public List<Vector2Int> GeneratePath()
    {
        List<Vector2Int> path = new List<Vector2Int>();

        for (int i = 0; i < Waypoints.Count; i++)
        {
            int targetIndex = (i + 1) % Waypoints.Count;
            Vector2Int start = Waypoints[i];
            Vector2Int target = Waypoints[targetIndex];
            path.AddRange(GridUtils.MakePath(start, target, false));
        }
        return path;
    }
}
