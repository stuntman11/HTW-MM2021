using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StaticEnemy : EntityBehaviour
{
    public int LuringRange = 0;

    private Vector2Int startPos;
    private Vector2Int startDir;

    private int luringState = -1;
    private IEnumerator<Vector2Int> lurePath;
    private int luringIdle = 0;

    protected override void OnStart()
    {
        base.OnStart();
        startPos = GridPos;
        startDir = GridDir;
    }

    public bool IsLuring
    {
        get { return luringState != -1; }
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
            PathFinding finding = level.Path(GridPos, startPos);
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
        if (lurePath.MoveNext())
        {
            MoveTowards(lurePath.Current);
        }
        else
        {
            RotateTo(startDir);
            luringState = -1;
        }
    }
}
