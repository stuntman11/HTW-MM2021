using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerController : EntityBehaviour
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
        
        TryMove(moveDir);
    }

    private void OnAfterTick()
    {
        bool playerHitLight = level.IsLightTile(pos);
        Debug.Log("Did the player hit the light?: " + playerHitLight);
        if (playerHitLight)
        {
            SceneManager.LoadScene("GameOverScreen");
        }
        
    }
}
