using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public Transform target;

    void Update()
    {
        Vector2 nextPos = Vector2.Lerp(transform.position, target.position, Time.deltaTime);
        transform.position = new Vector3(nextPos.x, nextPos.y, -1);
    }
}
