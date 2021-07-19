using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// A utility class that contains operations on a grid
/// </summary>
public static class GridUtils
{
    private static readonly float SQRT2 = Mathf.Sqrt(2);
    private static readonly float ROT_EPSILON = 0.01f;

    /// <summary>
    /// Calculates the angle of a grid unit direction
    /// </summary>
    /// <param name="direction">Unit direction</param>
    /// <returns>Angle in degrees</returns>
    public static float GetAngle(Vector2Int direction)
    {
        float rad = Mathf.Atan2(direction.y, direction.x);
        return Mathf.Rad2Deg * rad - 90;
    }

    /// <summary>
    /// Calculates the grid unit direction by an angle
    /// </summary>
    /// <param name="angle">Angle in degrees</param>
    /// <returns>Unit direction</returns>
    public static Vector2Int UnitDirection(float angle)
    {
        float rad = Mathf.Deg2Rad * (angle + 90);
        Vector2 rotation = new Vector2(Mathf.Cos(rad), Mathf.Sin(rad));
        return UnitDirection(rotation);
    }

    /// <summary>
    /// Calculates a grid unit direction by an arbitrary two dimensional direction
    /// </summary>
    /// <param name="direction">2D direction</param>
    /// <returns>Unit direction on the grid</returns>
    public static Vector2Int UnitDirection(Vector2 direction)
    {
        Vector2Int unitDirection = new Vector2Int();

        if (direction.x > ROT_EPSILON) unitDirection.x = 1;
        else if (direction.x < -ROT_EPSILON) unitDirection.x = -1;
        else if (direction.y > ROT_EPSILON) unitDirection.y = 1;
        else if (direction.y < -ROT_EPSILON) unitDirection.y = -1;

        return unitDirection;
    }

    /// <summary>
    /// Calculates the unit direction from a source position to a target position
    /// </summary>
    /// <param name="pos">Source position</param>
    /// <param name="target">Target position</param>
    /// <returns>Unit direction</returns>
    public static Vector2Int UnitDirection(Vector2Int pos, Vector2Int target)
    {
        return UnitDirection(target - pos);
    }

    /// <summary>
    /// Caclulates the distance between to positions on the grid.
    /// </summary>
    /// <param name="pos1">First position</param>
    /// <param name="pos2">Second position</param>
    /// <returns>Distance in tiles</returns>
    public static float DistanceBetween(Vector2Int pos1, Vector2Int pos2)
    {
        int dx = Mathf.Abs(pos1.x - pos2.x);
        int dy = Mathf.Abs(pos1.y - pos2.y);
        int diagonal = Mathf.Min(dx, dy);
        return diagonal * SQRT2 + (dx - diagonal) + (dy - diagonal);
    }

    /// <summary>
    /// Calculates a direct path as a list of absolute positions from a start position to an end position.
    /// </summary>
    /// <param name="start">Start position</param>
    /// <param name="end">End position</param>
    /// <param name="doIncludeEnd">True, if the end position should be included</param>
    /// <returns>List of positions</returns>
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
