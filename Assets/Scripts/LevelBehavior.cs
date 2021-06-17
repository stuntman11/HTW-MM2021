using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelBehavior : MonoBehaviour
{
    protected LevelController level;
    protected Vector2Int currentPos;
    protected Vector2Int lastPos;
    protected float timer;

    void Awake()
    {
        level = GameObject.Find("Controller").GetComponent<LevelController>();
        level.OnTick += OnLevelTick;
        OnStart();
    }

    // Update is called once per frame
    void Update()
    {
        timer += Time.deltaTime;
        Vector3 current = level.GridToWorldPos(currentPos);
        Vector3 last = level.GridToWorldPos(lastPos);
        transform.position = Vector3.Lerp(last, current, timer * 5);
    }

    private void OnLevelTick(string command)
    {
        lastPos = currentPos;
        timer = 0;
        OnTick(command);
    }

    protected Vector2Int DirectionTowards(Vector2Int targetPos)
    {
        Vector2Int direction = targetPos - currentPos;
        Vector2Int unitDirection = new Vector2Int();

        if (direction.x > 0) unitDirection.x = 1;
        else if (direction.x < 0) unitDirection.x = -1;
        else if (direction.y > 0) unitDirection.y = 1;
        else if (direction.y < 0) unitDirection.y = -1;

        return unitDirection;
    }

    protected void MoveTowards(Vector2Int targetPos)
    {
        currentPos += DirectionTowards(targetPos);
    }

    protected bool TryMove(Vector2Int direction)
    {
        Vector2Int nextPos = currentPos + direction;
        if (level.IsTileSolid(nextPos)) return false;
        currentPos = nextPos;
        return true;
    }

    protected void StartAt(Vector2Int pos)
    {
        currentPos = pos;
        lastPos = pos;
        transform.position = level.GridToWorldPos(pos);
    }

    protected void StartAtTransform() => StartAt(level.WorldToGridPos(transform.position));

    protected virtual void OnStart() { }
    protected virtual void OnTick(string command) { }
}
