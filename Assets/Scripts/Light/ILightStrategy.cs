using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ILightStrategy
{
    List<Vector4> CalculateRays(Vector2Int direction);
}
