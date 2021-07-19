using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Represents a path by key positions aka. waypoints.
/// Generates from Start to End position and forms a full cycle.
/// </summary>
public class CircularPath : MonoBehaviour, IPathGenerator
{
    /// <summary>List of waypoints</summary>
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
