using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConeLight : MonoBehaviour, ILightStrategy
{
    public int Length;

    public List<Vector4> CalculateRays(Vector2Int direction)
    {
        List<Vector4> rays = new List<Vector4>();
        Vector2 normal = new Vector2(direction.y, direction.x);

        for (int i = -Length; i <= Length; i++)
        {
            Vector2 offset = direction * (Length - 1) + normal * i;
            
            rays.Add(new Vector4(direction.x, direction.y, direction.x + offset.x, direction.y + offset.y));
        }
        return rays;
    }
}
