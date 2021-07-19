using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Applies executed moves to the player
/// </summary>
public class PlayerController : EntityBehaviour
{
    protected override void OnTick(Move move)
    {
        Vector2Int moveDir = Vector2Int.zero;

        if (move == Move.Up) moveDir = Vector2Int.up;
        else if (move == Move.Down) moveDir = Vector2Int.down;
        else if (move == Move.Left) moveDir = Vector2Int.left;
        else if (move == Move.Right) moveDir = Vector2Int.right;

        RotateTo(moveDir);
        TryMoveTowards(moveDir);
    }
}
