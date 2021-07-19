using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Manages the camera
/// </summary>
public class CameraController : MonoBehaviour
{
    /// <summary>Reference to the target of the camera</summary>
    public Transform target;

    private void Start()
    {
        transform.position = target.position;
    }

    void Update()
    {
        Vector2 nextPos = Vector2.Lerp(transform.position, target.position, Time.deltaTime);
        transform.position = new Vector3(nextPos.x, nextPos.y, -5);
    }
}
