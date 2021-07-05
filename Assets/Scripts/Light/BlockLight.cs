using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlockLight : MonoBehaviour, ILightStrategy
{
    public int Width;
    public int Length;
    public bool HasOffset;

    public List<Vector4> CalculateRays(Vector2Int direction)
    {
        List<Vector4> rays = new List<Vector4>();
        Vector2Int normal = new Vector2Int(direction.y, direction.x);
        Vector2Int offset = Vector2Int.zero;

        if (HasOffset) offset = direction;
       
        for (int i = -Width; i <= Width; i++)
        {
            Vector2 cross = normal * i + offset;
            Vector2 same = direction * Length;
            rays.Add(new Vector4(cross.x, cross.y, cross.x + same.x, cross.y + same.y));
        }
        return rays;
    }
}
