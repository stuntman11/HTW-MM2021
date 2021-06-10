using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class EnemyMovement : MonoBehaviour
{
   public enum PathType
    {
        Circular,
        Edge
    };

    public Tilemap tilemap;

    public PathType pathType;

    public List<Vector2Int> waypoints;

    private int targetIndex = 1;
    private int moveDir = 1;

    private Vector2Int currentPos;
    private Vector2Int lastPos;

    private float timer;
   

    // Start is called before the first frame update
    void Start()
    {
        transform.position = GridToWorldPos(waypoints[0]);
        currentPos = waypoints[0];
        lastPos = waypoints[0];
    }

    // Update is called once per frame
    void Update()
    {
        timer += Time.deltaTime;

        if (timer >= 1)
        {
            timer -= 1;
            Tick();
        }
        Vector3 current = GridToWorldPos(currentPos);
        Vector3 last = GridToWorldPos(lastPos);
        transform.position = Vector3.Lerp(last, current, timer * 5);
    }

    private void Tick()
    {
        Vector2Int targetWaypoint = waypoints[targetIndex];
        lastPos = currentPos;

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
        targetWaypoint = waypoints[targetIndex];
        currentPos = TileBasedMovement(targetWaypoint);
    }

    private Vector2Int TileBasedMovement(Vector2Int targetPos)
    {
        Vector2Int deltaVector = targetPos - currentPos;
        Vector2Int tbmVector = new Vector2Int(); //TileBasedMovementVector

        if (deltaVector.x > 0) tbmVector.x = 1;
        else if (deltaVector.x < 0) tbmVector.x = -1;
        else if (deltaVector.y > 0) tbmVector.y = 1;
        else if (deltaVector.y < 0) tbmVector.y = -1;

        return tbmVector + currentPos;
    }

    private Vector3 GridToWorldPos(Vector2Int grid)
    {
        int x = grid.x + tilemap.cellBounds.xMin;
        int y = grid.y + tilemap.cellBounds.yMin;
        Vector3 pos = tilemap.CellToWorld(new Vector3Int(x, y, 0));
        pos.z = transform.position.z;
        return pos;
    }
}
