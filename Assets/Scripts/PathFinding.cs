using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Represents an A* path finding task in the world
/// </summary>
public class PathFinding
{
    /// <summary>Start position</summary>
    public readonly Vector2Int Start;
    /// <summary>Target position</summary>
    public readonly Vector2Int Target;

    private LevelController controller;
    private List<PathNode> open;
    private ISet<Vector2Int> closed;
    private List<Vector2Int> path;
    private float distance;

    /// <summary>Calculated best path</summary>
    public IReadOnlyList<Vector2Int> Path
    {
        get { return path; }
    }

    /// <summary>Calculated best distance</summary>
    public float Distance
    {
        get { return distance; }
    }

    /// <summary>
    /// Creates a level driven PathFinding object
    /// </summary>
    /// <param name="controller">Instance of the LeveController</param>
    /// <param name="start">Start position</param>
    /// <param name="target">Target position</param>
    public PathFinding(LevelController controller, Vector2Int start, Vector2Int target)
    {
        this.controller = controller;
        this.Start = start;
        this.Target = target;
    }

    /// <summary>
    /// Calculates the best path via A* path finding.
    /// </summary>
    public void Execute()
    {
        open = new List<PathNode>();
        closed = new HashSet<Vector2Int>();
        path = null;

        if (IsWalkable(Start))
        {
            open.Add(new PathNode(Start));
        }
        
        while (open.Count > 0)
        {
            PathNode node = PopBestNode();

            if (node.Pos == Target)
            {
                distance = node.Gcost;
                TracePath(node);
                break;
            }
            closed.Add(node.Pos);
            AddNeighbors(node);
        }
    }

    private void TracePath(PathNode node)
    {
        PathNode current = node;
        List<Vector2Int> tracedPath = new List<Vector2Int>();

        while (current.Parent != null)
        {
            tracedPath.Add(current.Pos);
            current = current.Parent;
        }
        tracedPath.Reverse();
        path = tracedPath;
    }

    private void AddNeighbors(PathNode node)
    {
        for (int x = -1; x <= 1; x++)
        {
            for (int y = -1; y <= 1; y++)
            {
                int s = Mathf.Abs(x) + Mathf.Abs(y);
                if (s == 0 || s == 2) continue;
                Vector2Int offset = new Vector2Int(x, y);
                Vector2Int nPos = node.Pos + offset;
                if (!IsWalkable(nPos)) continue;
                PathNode nNode = open.Find((n) => n.Pos == nPos);
                float gcost = node.Gcost + GridUtils.DistanceBetween(node.Pos, nPos);
                float hcost = GridUtils.DistanceBetween(nPos, Target);

                if (nNode != null)
                {
                    if (gcost < nNode.Gcost)
                    {
                        nNode.Parent = node;
                        nNode.Gcost = gcost;
                        nNode.Hcost = hcost;
                    }
                }
                else
                {
                    nNode = new PathNode(nPos);
                    nNode.Parent = node;
                    nNode.Gcost = gcost;
                    nNode.Hcost = hcost;
                    open.Add(nNode);
                }
            }
        }
    }

    private bool IsWalkable(Vector2Int pos)
    {
        return !controller.IsTileSolid(pos) && !closed.Contains(pos);
    }

    private PathNode PopBestNode()
    {
        float bestCost = float.PositiveInfinity;
        int bestIndex = 0;

        for (int i = 0; i < open.Count; i++)
        {
            if (open[i].Cost < bestCost)
            {
                bestCost = open[i].Cost;
                bestIndex = i;
            }
        }
        PathNode node = open[bestIndex];
        open.RemoveAt(bestIndex);
        return node;
    }

    private class PathNode
    {
        public PathNode Parent;
        public Vector2Int Pos;
        public float Gcost;
        public float Hcost;

        public PathNode(Vector2Int pos)
        {
            this.Parent = null;
            this.Pos = pos;
            this.Gcost = 0;
            this.Hcost = 0;
        }

        public float Cost
        {
            get { return Gcost + Hcost; }
        }
    }
}
