using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Manages the collider of an item
/// </summary>
public class ItemScript : EntityBehaviour
{
    private void OnTriggerEnter2D(Collider2D other)
    {
        Destroy(gameObject);
        level.AddItem();
    }
}
