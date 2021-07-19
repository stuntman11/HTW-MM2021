using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Creates a translation layer between Unitys MonoBehavior and the game world.
/// Translates position, rotation between world and grid coordinates.
/// Provides access to the level and game events
/// </summary>
public class EntityBehaviour : MonoBehaviour
{
    /// <summary>Radius of the Jammer effect in tiles</summary>
    public static readonly float JAMMER_LEN = 6;
    /// <summary>Time of the Jammer effect in ticks</summary>
    public static readonly int JAMMER_TICKS = 8;

    /// <summary>Reference to the LevelController</summary>
    protected LevelController level;
    /// <summary>Current position in grid coordinates</summary>
    protected Vector2Int pos;
    /// <summary>Last position in grid coordinates</summary>
    protected Vector2Int lastPos;
    /// <summary>Current view direction on the grid</summary>
    protected Vector2Int direction;

    private int dizzyTicks = 0;

    /// <summary>Position on the grid</summary>
    public Vector2Int GridPos
    {
        get { return pos; }
    }

    /// <summary>Direction on the grid</summary>
    public Vector2Int GridDir
    {
        get { return direction; }
    }

    /// <summary>True, if entity is effected by a jammer</summary>
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

    /// <summary>
    /// An event that is executed when a level starts
    /// </summary>
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

    /// <summary>
    /// Sets the entities rotation by the specified direction
    /// </summary>
    /// <param name="direction">Unit direction</param>
    protected void RotateTo(Vector2Int direction)
    {
        if (direction == Vector2Int.zero) return;
        this.direction = direction;
        float degrees = GridUtils.GetAngle(direction);
        transform.rotation = Quaternion.Euler(0, 0, degrees);
    }

    /// <summary>
    /// Rotates the entity in the direction of the target position
    /// </summary>
    /// <param name="targetPos">Target position</param>
    protected void RotateTowards(Vector2Int targetPos)
    {
        RotateTo(GridUtils.UnitDirection(pos, targetPos));
    }

    /// <summary>
    /// Moves the entity a single tile towards the target position
    /// </summary>
    /// <param name="targetPos"></param>
    protected void MoveTowards(Vector2Int targetPos)
    {
        Vector2Int direction = GridUtils.UnitDirection(pos, targetPos);
        pos += direction;
        RotateTo(direction);
    }

    /// <summary>
    /// Tries to move the entity a single tile towards the target position.
    /// Fails if a non walkable tile is in front of the the entity.
    /// </summary>
    /// <param name="direction">Unit Direction</param>
    /// <returns>True if move was successfull</returns>
    protected bool TryMoveTowards(Vector2Int direction)
    {
        Vector2Int nextPos = pos + direction;
        if (level.IsTileSolid(nextPos)) return false;
        pos = nextPos;
        return true;
    }

    /// <summary>
    /// Sets the entities position to the specified position
    /// </summary>
    /// <param name="pos">Target position</param>
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

    /// <summary>
    /// An event that is executed when a move caused a world tick
    /// </summary>
    /// <param name="move">Executed move</param>
    protected virtual void OnTick(Move move) { }
}
