using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Represents a light geometry
/// </summary>
public interface ILightStrategy
{
    /// <summary>
    /// Calculates all light rays as a list of Vector4 depending on the view direction.
    /// A ray is encoded like: (source x, source y, destination x, destination y)
    /// A view direction is a unit vector based on the world grid
    /// </summary>
    /// <param name="direction">View direction</param>
    /// <returns>List of light rays</returns>
    List<Vector4> CalculateRays(Vector2Int direction);
}
