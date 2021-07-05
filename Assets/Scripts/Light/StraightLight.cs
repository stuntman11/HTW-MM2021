using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StraightLight : MonoBehaviour, ILightStrategy
{
    public int Length;

    public List<Vector4> CalculateRays(Vector2Int direction)
    {
        Debug.Log("Direction: " + direction);
        List<Vector4> rays = new List<Vector4>();
        Vector2 dest = direction * Length;
        rays.Add(new Vector4(0, 0, dest.x, dest.y));
        return rays;
    }
}
