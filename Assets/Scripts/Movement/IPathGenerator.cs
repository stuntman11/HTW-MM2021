using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IPathGenerator
{
    List<Vector2Int> GeneratePath();
}
