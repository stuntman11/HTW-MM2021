using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CustomLight : MonoBehaviour, ILightStrategy
{
    public List<Vector2> RayList;

    public List<Vector4> CalculateRays(Vector2Int direction) 
    {
        List<Vector4> rays = new List<Vector4>();
       
        foreach(Vector2 ray in RayList)
        {
            float rayRotation = Mathf.Atan2(direction.y, direction.x) - Mathf.PI/2;
            float rx = ray.x * Mathf.Cos(rayRotation) - ray.y * Mathf.Sin(rayRotation);
            float ry = ray.x * Mathf.Sin(rayRotation) + ray.y * Mathf.Cos(rayRotation);

            rays.Add(new Vector4(direction.x * 1.4f, direction.y * 1.4f, direction.x + rx, direction.y + ry));
        }
        return rays;

    }
}
