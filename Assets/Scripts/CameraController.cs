using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public Transform target;

    void Update()
    {
        Vector3 nextPos = Vector3.Lerp(transform.position, target.position, Time.deltaTime);
        nextPos.z = transform.position.z;
        transform.position = nextPos;
    }
}
