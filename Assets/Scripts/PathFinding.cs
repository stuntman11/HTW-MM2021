using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathFinding
{
    public readonly Vector2Int Start;
    public readonly Vector2Int Target;

    public PathFinding(Vector2Int start, Vector2Int target)
    {
        this.Start = start;
        this.Target = target;
    }

    public List<Vector2Int> FindPath()
    {
        return null;
    }
}
