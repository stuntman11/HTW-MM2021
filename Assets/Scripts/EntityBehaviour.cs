using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EntityBehaviour : MonoBehaviour
{
    public static readonly float ROT_EPSILON = 0.01f;
    public static readonly float JAMMER_LEN = 6;
    public static readonly int JAMMER_TICKS = 8;

    protected LevelController level;
    protected Vector2Int pos;
    protected Vector2Int lastPos;
    protected Vector2Int direction;

    private int dizzyTicks = 0;

    public Vector2Int GridPos
    {
        get { return pos; }
    }

    public Vector2Int GridDir
    {
        get { return direction; }
    }

    public bool IsDizzy
    {
        get { return dizzyTicks > 0; }
    }

    void Awake()
    {
        level = GameObject.Find("Controller").GetComponent<LevelController>();
        level.OnStart += OnStart;
        level.OnTick += OnLevelTick;
    }

    protected virtual void OnStart()
    {
        SetPosition(level.WorldToGridPos(transform.position));
        direction = GridUtils.UnitDirection(transform.rotation.eulerAngles.z);
    }

    void Update()
    {
        Vector3 current = level.GridToWorldPos(pos);
        Vector3 last = level.GridToWorldPos(lastPos);
        transform.position = Vector3.Lerp(last, current, level.TickProgress * 2);
    }

    private void OnLevelTick(Move move)
    {
        lastPos = pos;

        if (move == Move.Activate)
        {
            if (GridUtils.DistanceBetween(level.PlayerPos, GridPos) <= JAMMER_LEN)
            {
                dizzyTicks = JAMMER_TICKS;
            }
        }
        OnTick(move);
        dizzyTicks = Mathf.Max(dizzyTicks - 1, 0);
    }

    protected void RotateTo(Vector2Int direction)
    {
        if (direction == Vector2Int.zero) return;
        this.direction = direction;
        float degrees = GridUtils.GetAngle(direction);
        transform.rotation = Quaternion.Euler(0, 0, degrees);
    }

    protected void RotateTowards(Vector2Int targetPos)
    {
        RotateTo(GridUtils.UnitDirection(pos, targetPos));
    }

    protected void MoveTowards(Vector2Int targetPos)
    {
        Vector2Int direction = GridUtils.UnitDirection(pos, targetPos);
        pos += direction;
        RotateTo(direction);
    }

    protected bool TryMoveTowards(Vector2Int direction)
    {
        Vector2Int nextPos = pos + direction;
        if (level.IsTileSolid(nextPos)) return false;
        pos = nextPos;
        return true;
    }

    protected void SetPosition(Vector2Int pos)
    {
        this.pos = pos;
        lastPos = pos;
        transform.position = level.GridToWorldPos(pos);
    }

    void OnDestroy()
    {
        level.OnStart -= OnStart;
        level.OnTick -= OnLevelTick;
    }

    protected virtual void OnTick(Move move) { }
}
