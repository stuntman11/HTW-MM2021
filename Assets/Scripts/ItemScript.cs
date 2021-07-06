using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemScript : EntityBehaviour
{
    protected void OnTriggerEnter2D(Collider2D other)
    {
        Destroy(gameObject);
        level.AddItem();
    }
}
