using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Windows.Speech;
using UnityEngine.SceneManagement;
using UnityEngine.Tilemaps;
using TMPro;

public class LevelController : MonoBehaviour
{
    public delegate void TickEventHandler(Move move);
    public delegate void AfterTickEventHandler();
    public const float TICK_TIME = 0.5f;

    public float TickProgress
    {
       get { return Mathf.Clamp01(timer * 1.0f / TICK_TIME);  }
    }

    public GameObject Map;
    public GameObject EmptyTilemap;
    public TileBase LightTile;

    public int Score;

    public event TickEventHandler OnTick;
    public event AfterTickEventHandler OnAfterTick;

    private Queue<Move> moves = new Queue<Move>();
    private float timer = 0;
    private ActionParser parser = new ActionParser();

    private Tilemap environment;
    private Tilemap lights;
    private Tilemap paths;

    private TextMeshProUGUI commandText;
    private TextMeshProUGUI scoreText;

    void Awake()
    {
        environment = Map.transform.Find("Environment").GetComponent<Tilemap>();

        lights = Instantiate(EmptyTilemap, Map.transform).GetComponent<Tilemap>();
        lights.name = "Light";

        paths = Instantiate(EmptyTilemap, Map.transform).GetComponent<Tilemap>();
        paths.name = "Paths";

        commandText = GameObject.Find("Command").GetComponent<TextMeshProUGUI>();
        commandText.SetText("");

        scoreText = GameObject.Find("Score").GetComponent<TextMeshProUGUI>();
        scoreText.SetText(Score.ToString());

        CommandController command = GetComponent<CommandController>();
        command.OnCommand += OnCommand;
    }

    void Update()
    {
        timer += Time.deltaTime;

        if (moves.Count > 0 && timer >= TICK_TIME)
        {
            timer = 0;
            Move move = moves.Dequeue();
            lights.ClearAllTiles();
            OnTick.Invoke(move);
            AlterScore(-50);
            Debug.Log("Score: " + Score);
            OnAfterTick.Invoke();
        }
    }

    private void ValidateTilemaps()
    {
        bool hasFail = false;
        hasFail |= (lights.origin != environment.origin);
        hasFail |= (lights.cellBounds != environment.cellBounds);
        if (hasFail) throw new ArgumentException("tilemaps are not matching");
    }

    public Vector3 GridToWorldPos(Vector2Int pos)
    {
        Vector3 world = environment.GetCellCenterWorld(DenormalizeGrid(pos));
        world.z = 0;
        return world;
    }

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

    public void SetTile(Tilemap tilemap, Vector2Int pos, TileBase tile)
    {
        Vector3Int grid = DenormalizeGrid(pos);
        tilemap.SetTile(grid, tile);
    }

    public bool IsTileSolid(Vector2Int pos)
    {
        Tile tile = TileAt(environment, pos);
        if (tile == null) return true;
        return tile.colliderType == Tile.ColliderType.Grid;
    }

    public bool IsLightTile(Vector2Int pos)
    {
        return TileAt(lights, pos) != null;
    }

    public void ShineLight(Vector2Int pos, Vector2Int direction)
    {
        Vector2Int targetPos = pos + direction;
        SetTile(lights, targetPos, LightTile);
    }

    private void OnCommand(string command)
    {
        List<Move> nextMoves = parser.Parse(command);

        if (nextMoves != null)
        {
            commandText.SetText(command);

            foreach (Move move in nextMoves)
            {
                moves.Enqueue(move);
            }
        }
    }

    public void AlterScore(int changeValue)
    {
        Score = Mathf.Max(Score + changeValue, 0);
        scoreText.SetText(Score.ToString());
    }

    private Vector3Int DenormalizeGrid(Vector2Int n) => new Vector3Int(n.x + environment.origin.x, n.y + environment.origin.y, 0);
    private Vector2Int NormalizeGrid(Vector3Int n) => new Vector2Int(n.x - environment.origin.x, n.y - environment.origin.y);
}
