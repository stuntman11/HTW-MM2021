using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingEnemy : EntityBehaviour
{
    private List<Vector2Int> path;
    private int walkIndex = 0;

    public IEnumerable<Vector2Int> Path
    {
        get { return path; }
    }

    protected override void OnStart()
    {
        base.OnStart();
        IPathGenerator generator = GetComponent<IPathGenerator>();
        path = generator.GeneratePath();
        SetPosition(path[walkIndex]);
    }

    protected override void OnTick(Move move)
    {
        walkIndex = (walkIndex + 1) % path.Count;
        MoveTowards(path[walkIndex]);
    }
}
