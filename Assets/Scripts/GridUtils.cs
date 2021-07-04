using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class GridUtils
{
    private static readonly float SQRT2 = Mathf.Sqrt(2);
    private static readonly float ROT_EPSILON = 0.01f;

    public static float GetAngle(Vector2Int direction)
    {
        float rad = Mathf.Atan2(direction.y, direction.x);
        return Mathf.Rad2Deg * rad - 90;
    }

    public static Vector2Int UnitDirection(float angle)
    {
        float rad = Mathf.Deg2Rad * (angle + 90);
        Vector2 rotation = new Vector2(Mathf.Cos(rad), Mathf.Sin(rad));
        return UnitDirection(rotation);
    }

    public static Vector2Int UnitDirection(Vector2 direction)
    {
        Vector2Int unitDirection = new Vector2Int();

        if (direction.x > ROT_EPSILON) unitDirection.x = 1;
        else if (direction.x < -ROT_EPSILON) unitDirection.x = -1;
        else if (direction.y > ROT_EPSILON) unitDirection.y = 1;
        else if (direction.y < -ROT_EPSILON) unitDirection.y = -1;

        return unitDirection;
    }

    public static Vector2Int UnitDirection(Vector2Int pos, Vector2Int target)
    {
        return UnitDirection(target - pos);
    }

    public static float DistanceBetween(Vector2Int pos1, Vector2Int pos2)
    {
        int dx = Mathf.Abs(pos1.x - pos2.x);
        int dy = Mathf.Abs(pos1.y - pos2.y);
        int diagonal = Mathf.Min(dx, dy);
        return diagonal * SQRT2 + (dx - diagonal) + (dy - diagonal);
    }

    public static List<Vector2Int> MakePath(Vector2Int start, Vector2Int end, bool doIncludeEnd)
    {
        List<Vector2Int> path = new List<Vector2Int>();
        Vector2Int pos = start;

        while (pos != end)
        {
            path.Add(pos);
            pos += UnitDirection(pos, end);
        }

        if (doIncludeEnd) path.Add(end);
        return path;
    }
}
