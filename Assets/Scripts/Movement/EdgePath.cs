using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EdgePath : MonoBehaviour, IPathGenerator
{
    public List<Vector2Int> Waypoints;

    public List<Vector2Int> GeneratePath()
    {
        List<Vector2Int> path = new List<Vector2Int>();

        for (int i = 0; i < Waypoints.Count - 1; i++)
        {
            Vector2Int start = Waypoints[i];
            Vector2Int end = Waypoints[i + 1];
            path.AddRange(GridUtils.MakePath(start, end, false));
        }

        for (int i = Waypoints.Count - 1; i >= 1; i--)
        {
            Vector2Int start = Waypoints[i];
            Vector2Int end = Waypoints[i - 1];
            path.AddRange(GridUtils.MakePath(start, end, false));
        }
        return path;
    }
}
