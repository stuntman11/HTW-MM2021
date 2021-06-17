using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMovement : LevelBehavior
{
    public enum PathType
    {
        Circular,
        Edge
    };
    
    public PathType pathType;
    public List<Vector2Int> waypoints;

    private int targetIndex = 1;
    private int moveDir = 1;

    protected override void OnStart()
    {
        StartAt(waypoints[0]);
    }

    protected override void OnTick(string command)
    {
        Vector2Int targetWaypoint = waypoints[targetIndex];
        
        if (currentPos.x == targetWaypoint.x && currentPos.y == targetWaypoint.y)
        {
            if (pathType == PathType.Circular)
            {
                targetIndex = (targetIndex + 1) % waypoints.Count;
            }
            else if (pathType == PathType.Edge)
            {
                if (targetIndex + moveDir < 0) moveDir = 1;
                else if (targetIndex + moveDir >= waypoints.Count) moveDir = -1;

                targetIndex += moveDir;
            }
        }
        int nextTargetIndex = Mathf.Clamp(targetIndex, 0, waypoints.Count - 1);
        targetWaypoint = waypoints[nextTargetIndex];
        RotateTowards(targetWaypoint);
        MoveTowards(targetWaypoint);
    }
}
