using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EntityBehaviour : MonoBehaviour
{
    public const float ROT_EPSILON = 0.01f;

    protected LevelController level;
    protected Vector2Int pos;
    protected Vector2Int lastPos;
    protected Vector2Int direction;

    public Vector2Int Pos
    {
        get { return pos; }
    }

    public Vector2Int Direction
    {
        get { return direction; }
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

        float rad = Mathf.Deg2Rad * (transform.rotation.z + 90);
        Vector2 rotation = new Vector2(Mathf.Cos(rad), Mathf.Sin(rad));
        direction = GetUnitDirection(rotation);
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
        OnTick(move);
    }

    protected Vector2Int GetUnitDirection(Vector2 direction)
    {
        Vector2Int unitDirection = new Vector2Int();

        if (direction.x > ROT_EPSILON) unitDirection.x = 1;
        else if (direction.x < -ROT_EPSILON) unitDirection.x = -1;
        else if (direction.y > ROT_EPSILON) unitDirection.y = 1;
        else if (direction.y < -ROT_EPSILON) unitDirection.y = -1;

        return unitDirection;
    }

    protected Vector2Int GetDirectionTowards(Vector2Int targetPos)
    {
        Vector2Int direction = targetPos - pos;
        return GetUnitDirection(direction);
    }

    protected void RotateTo(Vector2Int direction)
    {
        this.direction = direction;
        float angle = Mathf.Atan2(direction.y, direction.x);
        float degrees = Mathf.Rad2Deg * angle - 90;
        transform.rotation = Quaternion.Euler(0, 0, degrees);
    }

    protected void RotateTowards(Vector2Int targetPos)
    {
        Vector2Int direction = GetDirectionTowards(targetPos);
        RotateTo(direction);
    }

    protected void MoveTowards(Vector2Int targetPos)
    {
        Vector2Int direction = GetDirectionTowards(targetPos);
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

    protected virtual void OnTick(Move move) { }
}
