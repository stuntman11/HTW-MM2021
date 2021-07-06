using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollectableScipt : EntityBehaviour
{
    public int CollectableValue = 1000;
    public int VisibleRadius = 0;

    protected override void OnStart()
    {
        base.OnStart();

        if (VisibleRadius > 0)
        {
            level.OnTick += (move) => UpdateVisibility();
            UpdateVisibility();
        }
    }

    private void UpdateVisibility()
    {
        float distance = GridUtils.DistanceBetween(level.PlayerPos, GridPos);
        bool isVisible = distance <= VisibleRadius;
        gameObject.SetActive(isVisible);
    }

    protected void OnTriggerEnter2D(Collider2D other)
    {
        Destroy(gameObject);
        level.ChangeScoreBy(CollectableValue);
    }
}
