using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// Manages the collider of a finish trigger
/// </summary>
public class LevelFinishTrigger : EntityBehaviour
{
    private bool hasTriggered = false;

    protected override void OnStart()
    {
        base.OnStart();
        level.OnAfterTick += OnAfterTick;
    }

    private void OnAfterTick()
    {
        if (!hasTriggered) return;
        SceneManager.LoadScene("LevelFinished");
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        hasTriggered = true;
    }
}
