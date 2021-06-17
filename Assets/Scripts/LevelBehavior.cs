using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelBehavior : MonoBehaviour
{
    protected LevelController level;
    protected Vector2Int pos;
    protected Vector2Int lastPos;

    void Awake()
    {
        level = GameObject.Find("Controller").GetComponent<LevelController>();
        level.OnTick += OnLevelTick;
        OnStart();
    }

    void Update()
    {
        Vector3 current = level.GridToWorldPos(pos);
        Vector3 last = level.GridToWorldPos(lastPos);
        transform.position = Vector3.Lerp(last, current, level.TickTime * 2);
    }

    private void OnLevelTick(Move move)
    {
        lastPos = pos;
        OnTick(move);
    }

    protected Vector2Int DirectionTowards(Vector2Int targetPos)
    {
        Vector2Int direction = targetPos - pos;
        Vector2Int unitDirection = new Vector2Int();

        if (direction.x > 0) unitDirection.x = 1;
        else if (direction.x < 0) unitDirection.x = -1;
        else if (direction.y > 0) unitDirection.y = 1;
        else if (direction.y < 0) unitDirection.y = -1;

        return unitDirection;
    }

    protected void Rotate(Vector2Int direction)
    {
        float angle = Mathf.Atan2(direction.y, direction.x);
        float degrees = Mathf.Rad2Deg * angle - 90;
        transform.rotation = Quaternion.Euler(0, 0, degrees);
    }

    protected Vector2Int RotateTowards(Vector2Int targetPos)
    {
        Vector2Int direction = DirectionTowards(targetPos);
        Rotate(direction);
        return direction;
    }

    protected Vector2Int MoveTowards(Vector2Int targetPos)
    {
        Vector2Int direction = DirectionTowards(targetPos);
        pos += direction;
        return direction;
    }

    protected bool TryMove(Vector2Int direction)
    {
        Vector2Int nextPos = pos + direction;
        if (level.IsTileSolid(nextPos)) return false;
        pos = nextPos;
        return true;
    }

    protected void StartAt(Vector2Int pos)
    {
        this.pos = pos;
        lastPos = pos;
        transform.position = level.GridToWorldPos(pos);
    }

    protected void StartAtTransform() => StartAt(level.WorldToGridPos(transform.position));

    protected virtual void OnStart() { }
    protected virtual void OnTick(Move move) { }
}
