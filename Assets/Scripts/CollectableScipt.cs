using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollectableScipt : EntityBehaviour
{
    public int CollectableValue = 1000;

    protected override void OnStart()
    {
        StartAtTransform();
    }

    protected void OnTriggerEnter2D(Collider2D other)
    {
        Debug.Log("Collided");
        GameObject.Destroy(gameObject);
        level.AlterScore(CollectableValue);
    }
}
