using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : LevelBehavior
{
    protected override void OnStart()
    {
        StartAtTransform();
    }

    protected override void OnTick(string command)
    {
        Vector2Int move = Vector2Int.zero;

        if (command.Equals("hoch")) move = Vector2Int.up;
        else if (command.Equals("runter")) move = Vector2Int.down;
        else if (command.Equals("links")) move = Vector2Int.left;
        else if (command.Equals("rechts")) move = Vector2Int.right;

        bool hasMoved = TryMove(move);
        Debug.Log(string.Format("Has Moved: {0}", hasMoved));
    }
}
