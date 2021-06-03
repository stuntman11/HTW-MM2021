using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMovement : MonoBehaviour
{
   public enum PathType
    {
        Circular,
        Edge
    };

    public PathType pathType;

    public List<Vector2> waypoints;

    private int targetIndex = 1;
    private int moveDir = 1;

    private Vector3 currentPos;
    private Vector3 lastPos;

    private float timer;
   

    // Start is called before the first frame update
    void Start()
    {
        transform.position = waypoints[0];
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
            tick();
        }

        transform.position = Vector3.Lerp(lastPos, currentPos, timer * 5);

    }

    Vector3 tileBasedMovement(Vector3 targetPosition)
    {
        Vector3 deltaVector = targetPosition - currentPos;
        Vector3 tbmVector = new Vector3(0f, 0f, 0f); //TileBasedMovementVector

        if (deltaVector.x > 0) tbmVector.x = 2.56f;
        else if (deltaVector.x < 0) tbmVector.x = -2.56f;
        else if (deltaVector.y > 0) tbmVector.y = 2.56f;
        else if (deltaVector.y < 0) tbmVector.y = -2.56f;

        return tbmVector + currentPos;
    }

    void tick()
    {
        Vector2 targetWaypoint = waypoints[targetIndex];
        
        lastPos = currentPos;
        print("Current Waypoint: " + targetWaypoint);
        print("Current Index: " + targetIndex);
        print("Current Pos: " + currentPos);

        if(currentPos.x == targetWaypoint.x && currentPos.y == targetWaypoint.y) {
            if (pathType == PathType.Circular)
            {
                targetIndex = (targetIndex + 1) % waypoints.Count;
            }
            else if (pathType == PathType.Edge)
            {

                if (targetIndex + moveDir < 0)
                {
                    moveDir = 1;
                }
                else if (targetIndex + moveDir >= waypoints.Count)
                {
                    moveDir = -1;
                }
                targetIndex += moveDir;
            }
        }
        targetWaypoint = waypoints[targetIndex];
        currentPos = tileBasedMovement(targetWaypoint);
       
        /*
        lastPos = currentPos;
        if (currentPos != movePoint.position && !positionReached)
        {
            currentPos = tileBasedMovement(movePoint.position);
            if (currentPos == movePoint.position)
            {
                positionReached = true;
            }

        }
        else if (positionReached)
        {
            currentPos = tileBasedMovement(startingPosition);
            if (currentPos == startingPosition)
            {
                positionReached = false;
            }

        }*/
    }
}
