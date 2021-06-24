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
    public int Score = 5000;

    public float TickTime
    {
       get { return timer * 1.0f / TICK_TIME;  }
    }

    public Tilemap Environment;
    public Tilemap Light;
    public TileBase LightTile;
    public Transform Collectable;
    private TextMeshProUGUI scoreText;
    
    public event TickEventHandler OnTick;
    public event AfterTickEventHandler OnAfterTick;

    private Queue<Move> moves = new Queue<Move>();
    private float timer = 0;
    private ActionParser parser = new ActionParser();
    private PhraseRecognizer recognizer;
    private TextMeshProUGUI commandText;

    void Awake()
    {
        ValidateTilemaps();

        commandText = GameObject.Find("Command").GetComponent<TextMeshProUGUI>();
        commandText.SetText("");

        scoreText = GameObject.Find("Score").GetComponent<TextMeshProUGUI>();
        scoreText.SetText(Score.ToString());
        recognizer = new GrammarRecognizer(Application.streamingAssetsPath + "/grammar.xml", ConfidenceLevel.Rejected);
        recognizer.OnPhraseRecognized += OnRecognition;
    }

    void Update()
    {
        timer += Time.deltaTime;

        if (moves.Count > 0 && timer >= TICK_TIME)
        {
            timer = 0;
            Move move = moves.Dequeue();
            Light.ClearAllTiles();
            OnTick.Invoke(move);
            AlterScore(-50);
            Debug.Log("Score: " + Score);
            OnAfterTick.Invoke();
        }
    }

    private void ValidateTilemaps()
    {
        bool hasFail = false;
        hasFail |= (Light.origin != Environment.origin);
        hasFail |= (Light.cellBounds != Environment.cellBounds);
        if (hasFail) throw new ArgumentException("tilemaps are not matching");
    }

    public Vector3 GridToWorldPos(Vector2Int pos)
    {
        Vector3 world = Environment.CellToWorld(DenormalizeGrid(pos));
        world.z = 0;
        return world;
    }

    public Vector2Int WorldToGridPos(Vector3 world)
    {
        Vector3Int grid = Environment.WorldToCell(new Vector3(world.x, world.y, 0));
        return NormalizeGrid(grid);
    }

    private Tile TileAt(Tilemap tilemap, Vector2Int pos)
    {
        Vector3Int grid = DenormalizeGrid(pos);
        return tilemap.GetTile<Tile>(grid);
    }

    public bool IsTileSolid(Vector2Int pos)
    {
        Tile tile = TileAt(Environment, pos);
        if (tile == null) return true;
        return tile.colliderType == Tile.ColliderType.Grid;
    }

    public bool IsLightTile(Vector2Int pos)
    {
        return TileAt(Light, pos) != null;
    }

    public void ShineLight(Vector2Int pos, Vector2Int direction)
    {
        Vector2Int targetPos = pos + direction;
        Debug.Log(string.Format("Light at: {0}, {1} from {2}, {3}", targetPos.x, targetPos.y, pos.x, pos.y));
        Light.SetTile(DenormalizeGrid(targetPos), LightTile);
    }

    private void OnRecognition(PhraseRecognizedEventArgs args)
    {
        string command = args.text;
        Debug.Log(string.Format("Command: '{0}'", command));
        
        if (command.Equals("pause"))
        {
            SceneManager.LoadScene("MainMenu");
        }
        else
        {
            commandText.SetText(command);
            List<Move> nextMoves = parser.Parse(command);

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

    void OnEnable() => recognizer.Start();
    void OnDisable() => recognizer.Stop();
    void OnDestroy() => recognizer.Dispose();

    private Vector3Int DenormalizeGrid(Vector2Int n) => new Vector3Int(n.x + Environment.origin.x, n.y + Environment.origin.y, 0);
    private Vector2Int NormalizeGrid(Vector3Int n) => new Vector2Int(n.x - Environment.origin.x, n.y - Environment.origin.y);
}
