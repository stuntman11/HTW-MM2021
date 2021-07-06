using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingEnemy : EntityBehaviour, IEnemy
{
    public int LuringRange = 0;

    private List<Vector2Int> path;
    private int walkIndex = 0;

    private int luringState = -1;
    private IEnumerator<Vector2Int> lurePath;
    private int luringIdle = 0;

    public IEnumerable<Vector2Int> Path
    {
        get { return path; }
    }

    public bool IsLuring
    {
        get { return luringState != -1; }
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
        if (move == Move.Lure)
        {
            PathFinding finding = level.Path(GridPos, level.PlayerPos);
            if (finding.Distance > LuringRange) return;
            lurePath = finding.Path.GetEnumerator();
            luringState = 0;
        }

        if (luringState == 0) UpdateLuring0();
        else if (luringState == 1) UpdateLuring1();
        else if (luringState == 2) UpdateLuring2();
        else UpdateWalk();
    }

    private void UpdateWalk()
    {
        walkIndex = (walkIndex + 1) % path.Count;
        MoveTowards(path[walkIndex]);
    }

    private void UpdateLuring0()
    {
        if (lurePath.MoveNext())
        {
            MoveTowards(lurePath.Current);
        }
        else
        {
            luringIdle = 3;
            luringState = 1;
        }
    }

    private void UpdateLuring1()
    {
        if (luringIdle == 0)
        {
            walkIndex = FindNearestToPath(GridPos);
            PathFinding finding = level.Path(GridPos, path[walkIndex]);
            lurePath = finding.Path.GetEnumerator();
            luringState = 2;
        }
        else
        {
            luringIdle--;
        }
    }

    private void UpdateLuring2()
    {
        if (lurePath.MoveNext()) MoveTowards(lurePath.Current);
        else luringState = -1;
    }

    public int FindNearestToPath(Vector2Int pos)
    {
        float bestDist = float.PositiveInfinity;
        int bestIndex = -1;

        for (int i = 0; i < path.Count; i++)
        {
            float distance = GridUtils.DistanceBetween(pos, path[i]);

            if (distance < bestDist)
            {
                bestIndex = i;
                bestDist = distance;
            }
        }
        return bestIndex;
    }
}
