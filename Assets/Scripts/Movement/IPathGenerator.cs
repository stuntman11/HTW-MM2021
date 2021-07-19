using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Represents a path strategy
/// </summary>
public interface IPathGenerator
{
    /// <summary>
    /// Calculates a list of absolute positions as Vector2 that represent the full path.
    /// Each position is a direct neighbor of its predecessor position.
    /// </summary>
    /// <returns>List of positions</returns>
    List<Vector2Int> GeneratePath();
}
