using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.SceneManagement;
using TMPro;

/// <summary>
/// Manages tilemaps, entities, light and path calculations
/// </summary>
public class LevelController : MonoBehaviour
{
    /// <summary>Describes a handler for the LevelStartEvent</summary>
    public delegate void LevelStartHandler();
    /// <summary>Describes a handler for the TickEvent</summary>
    public delegate void TickEventHandler(Move move);
    /// <summary>Describes a handler for the AfterTickEvent</summary>
    public delegate void AfterTickEventHandler();
    /// <summary>The duration each tick lasts in seconds</summary>
    public static readonly float TICK_TIME = 0.5f;

    /// <summary>Progress of a tick, ranging from 0 to 1</summary>
    public float TickProgress
    {
       get { return Mathf.Clamp01(timer * 1.0f / TICK_TIME);  }
    }

    /// <summary>Reference to the scenes map object</summary>
    public GameObject Map;
    /// <summary>Reference to the EmptyTilemap prefab</summary>
    public GameObject EmptyTilemap;
    /// <summary>Reference to the scenes player object</summary>
    public Transform Player;
    /// <summary>Reference to the jammer asset</summary>
    public GameObject JammerIndicator;
    /// <summary>Reference to the light tile asset</summary>
    public TileBase LightTile;
    /// <summary>Reference to the path tile asset</summary>
    public TileBase PathTile;

    /// <summary>Initial score of the level</summary>
    public int InitialScore;

    /// <summary>Event that is executed at the level start</summary>
    public event LevelStartHandler OnStart;
    /// <summary>Event that is executed each tick</summary>
    public event TickEventHandler OnTick;
    /// <summary>Event that is executed after a tick is fully completed</summary>
    public event AfterTickEventHandler OnAfterTick;

    private Queue<Move> moves = new Queue<Move>();
    private ActionParser parser = new ActionParser();
    private bool hasTicked = false;
    private float timer = 0;
    private int items = 0;

    private Tilemap environment;
    private Tilemap lights;
    private Tilemap paths;

    private TextMeshProUGUI commandText;
    private TextMeshProUGUI scoreText;

    private List<EntityBehaviour> lightEmitter = new List<EntityBehaviour>();
    private List<EntityBehaviour> finishTiles = new List<EntityBehaviour>();
    private List<EntityBehaviour> enemies = new List<EntityBehaviour>();

    /// <summary>The player position on the grid</summary>
    public Vector2Int PlayerPos
    {
        get { return Player.GetComponent<EntityBehaviour>().GridPos; }
    }

    void Awake()
    {
        MakeNoSound.Score = InitialScore;

        environment = Map.transform.Find("Environment").GetComponent<Tilemap>();

        lights = Instantiate(EmptyTilemap, Map.transform).GetComponent<Tilemap>();
        lights.name = "Light";
        lights.transform.position = new Vector3(0, 0, -1);

        paths = Instantiate(EmptyTilemap, Map.transform).GetComponent<Tilemap>();
        paths.name = "Paths";
        paths.transform.position = new Vector3(0, 0, -0.5f);

        commandText = GameObject.Find("Command").GetComponent<TextMeshProUGUI>();
        commandText.SetText("");

        scoreText = GameObject.Find("Score").GetComponent<TextMeshProUGUI>();
        scoreText.SetText(MakeNoSound.Score.ToString());

        JammerIndicator.SetActive(false);

        CommandController command = GetComponent<CommandController>();
        command.OnCommand += OnCommand;
    }

    void Start()
    {
        OnStart?.Invoke();

        foreach (EntityBehaviour entity in FindObjectsOfType<EntityBehaviour>())
        {
            ILightStrategy lightStrategy = entity.GetComponent<ILightStrategy>();
            IEnemy enemy = entity.GetComponent<IEnemy>();
            LevelFinishTrigger finish = entity.GetComponent<LevelFinishTrigger>();

            if (lightStrategy != null) lightEmitter.Add(entity);
            if (enemy != null) enemies.Add(entity);
            if (finish != null) finishTiles.Add(finish);
        }
        RecalculateLight();
        RecalculatePaths();
    }

    void Update()
    {
        timer += Time.deltaTime;

        if (hasTicked && timer >= TICK_TIME)
        {
            hasTicked = false;
            OnAfterTick?.Invoke();
            CheckGameLost();
        }

        if (moves.Count > 0 && timer >= TICK_TIME)
        {
            timer = 0;
            hasTicked = true;
            Move move = ConvertMove(moves.Dequeue());
            OnTick?.Invoke(move);
            ChangeScoreBy(-50);
            RecalculateLight();
            RecalculatePaths();
        }
    }

    private Move ConvertMove(Move move)
    {
        if (move == Move.Activate)
        {
            if (items == 0) return Move.Wait;
            items--;
            GameObject.Find("Audio/Zap").GetComponent<AudioSource>().Play();
            UpdateJammerIndicator();
        }
        return move;
    }

    /// <summary>
    /// Converts a 2d position on the grid to a 3d world position
    /// </summary>
    /// <param name="pos">Grid position</param>
    /// <returns>World position</returns>
    public Vector3 GridToWorldPos(Vector2Int pos)
    {
        Vector3 world = environment.GetCellCenterWorld(DenormalizeGrid(pos));
        world.z = -2;
        return world;
    }

    /// <summary>
    /// Converts a 3d world position to a 2d position on the grid
    /// </summary>
    /// <param name="world">World position</param>
    /// <returns>Grid position</returns>
    public Vector2Int WorldToGridPos(Vector3 world)
    {
        Vector3Int grid = environment.WorldToCell(new Vector3(world.x, world.y, 0));
        return NormalizeGrid(grid);
    }

    private Tile TileAt(Tilemap tilemap, Vector2Int pos)
    {
        Vector3Int grid = DenormalizeGrid(pos);
        return tilemap.GetTile<Tile>(grid);
    }

    private void SetTile(Tilemap tilemap, Vector2Int pos, TileBase tile)
    {
        Vector3Int grid = DenormalizeGrid(pos);
        tilemap.SetTile(grid, tile);
    }

    /// <summary>
    /// Returns if the tile at the specified position is solid.
    /// Solid states, that the player can not walk on this tile.
    /// </summary>
    /// <param name="pos">Grid position</param>
    /// <returns>True if the tile is solid</returns>
    public bool IsTileSolid(Vector2Int pos)
    {
        Tile tile = TileAt(environment, pos);
        if (tile == null) return true;
        return tile.colliderType != Tile.ColliderType.None;
    }

    /// <summary>
    /// Executes a A* path finding from a start position to a target position.
    /// </summary>
    /// <param name="start">Start position</param>
    /// <param name="target">Target position</param>
    /// <returns>A PathFinding object</returns>
    public PathFinding Path(Vector2Int start, Vector2Int target)
    {
        PathFinding finding = new PathFinding(this, start, target);
        finding.Execute();
        return finding;
    }

    private void RecalculateLight()
    {
        lights.ClearAllTiles();

        foreach (EntityBehaviour emitter in lightEmitter)
        {
            if (emitter.IsDizzy) continue;
            ILightStrategy strategy = emitter.GetComponent<ILightStrategy>();
            List<Vector4> rays = strategy.CalculateRays(emitter.GridDir);
            foreach (Vector4 ray in rays) LightTrace(emitter.GridPos, ray);
        }
    }

    private void RecalculatePaths()
    {
        paths.ClearAllTiles();

        foreach (EntityBehaviour enemy in enemies)
        {
            MovingEnemy moving = enemy.GetComponent<MovingEnemy>();
            if (moving == null) continue;

            foreach (Vector2Int pos in moving.Path)
            {
                SetTile(paths, pos, PathTile);
            }
        }
    }

    private void LightTrace(Vector2Int origin, Vector4 lightRay)
    {
        float x = lightRay.x;
        float y = lightRay.y;
        float dx = lightRay.z - lightRay.x;
        float dy = lightRay.w - lightRay.y;
        int i = 0;
        float step;

        if (Math.Abs(dx) >= Math.Abs(dy)) step = Math.Abs(dx);
        else step = Math.Abs(dy);

        dx /= step;
        dy /= step;

        while (i++ <= step)
        {
            Vector2Int offset = new Vector2Int((int)x, (int)y);
            Vector2Int lightPos = origin + offset;
            bool isSolid = IsTileSolid(lightPos);
            if (isSolid) break;
            SetTile(lights, lightPos, LightTile);
            x += dx;
            y += dy;
        }
    }

    private void CheckGameLost()
    {
        if (!IsTouchingFinish())
        {
            if (TileAt(lights, PlayerPos) != null || MakeNoSound.Score == 0 || IsTouchingEnemy())
            {
                SceneManager.LoadScene("GameOver");
            }
        }
    }

    private bool IsTouchingEnemy()
    {
        foreach (EntityBehaviour enemy in enemies)
        {
            if (enemy.GridPos == PlayerPos)
            {
                return true;
            }
        }
        return false;
    }

    private bool IsTouchingFinish()
    {
        foreach (EntityBehaviour finish in finishTiles)
        {
            if (finish.GridPos == PlayerPos)
            {
                return true;
            }
        }
        return false;
    }

    private void OnCommand(string command)
    {
        if (command.Equals("stop"))
        {
            moves.Clear();
            commandText.SetText(command);
            return;
        }

        List<Move> nextMoves = parser.Parse(command);
        if (nextMoves == null) return;
        commandText.SetText(command);

        foreach (Move move in nextMoves)
        {
            moves.Enqueue(move);
        }
    }

    /// <summary>
    /// Changes the level score by the specified amount
    /// </summary>
    /// <param name="change">Change amount</param>
    public void ChangeScoreBy(int change)
    {
        MakeNoSound.Score = Mathf.Max(MakeNoSound.Score + change, 0);
        scoreText.SetText(MakeNoSound.Score.ToString());
    }

    /// <summary>
    /// Adds a jammer item to the inventory
    /// </summary>
    public void AddItem()
    {
        items++;
        UpdateJammerIndicator();
    }

    private void UpdateJammerIndicator() => JammerIndicator.SetActive(items > 0);

    private Vector3Int DenormalizeGrid(Vector2Int n) => new Vector3Int(n.x + environment.origin.x, n.y + environment.origin.y, 0);
    private Vector2Int NormalizeGrid(Vector3Int n) => new Vector2Int(n.x - environment.origin.x, n.y - environment.origin.y);
}
