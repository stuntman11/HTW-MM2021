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
        /*
        EntityBehaviour player = level.Player.GetComponent<EntityBehaviour>();
        PathFinding finding = level.Path(GridPos, player.GridPos);

        if (finding.Path?.Count > 0)
        {
            RotateTowards(finding.Path[0]);
            SetPosition(finding.Path[0]);
        }
        */

        walkIndex = (walkIndex + 1) % path.Count;
        MoveTowards(path[walkIndex]);
    }

    public Vector2Int FindNearestToPath(Vector2Int pos)
    {
        float bestDist = float.PositiveInfinity;
        Vector2Int bestPath = Vector2Int.zero;

        foreach (Vector2Int singlePath in path)
        {
            float distance = GridUtils.DistanceBetween(pos, singlePath);

            if (distance < bestDist)
            {
                bestPath = singlePath;
                distance = bestDist;
            }
        }
        return bestPath;
    }
}
