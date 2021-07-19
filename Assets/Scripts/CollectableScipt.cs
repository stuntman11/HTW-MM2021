using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Manages properties and the collider of a collectable
/// </summary>
public class CollectableScipt : EntityBehaviour
{
    /// <summary>Score value of the collectable</summary>
    public int CollectableValue = 1000;
    /// <summary>Radius in which the collecable is visible</summary>
    public int VisibleRadius = 0;

    protected override void OnStart()
    {
        base.OnStart();
        UpdateVisibility();
    }

    private void UpdateVisibility()
    {
        float distance = GridUtils.DistanceBetween(level.PlayerPos, GridPos);
        bool isVisible = VisibleRadius == 0 || distance <= VisibleRadius;
        gameObject.SetActive(isVisible);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        GameObject.Find("Audio/Collect").GetComponent<AudioSource>().Play();
        Destroy(gameObject);
        level.ChangeScoreBy(CollectableValue);
    }
    protected override void OnTick(Move move) => UpdateVisibility();
}
