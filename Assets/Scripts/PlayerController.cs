using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : LevelBehavior
{
    protected override void OnStart()
    {
        StartAtTransform();
        level.OnAfterTick += OnAfterTick;
    }

    protected override void OnTick(Move move)
    {
        Vector2Int moveDir = Vector2Int.zero;

        if (move == Move.Up) moveDir = Vector2Int.up;
        else if (move == Move.Down) moveDir = Vector2Int.down;
        else if (move == Move.Left) moveDir = Vector2Int.left;
        else if (move == Move.Right) moveDir = Vector2Int.right;

        if(moveDir != Vector2Int.zero) Rotate(moveDir);
        bool hasMoved = TryMove(moveDir);
        
    }

    private void OnAfterTick()
    {
        Debug.Log("Did the player hit the light?: " + level.IsLightTile(pos));
    }
}
