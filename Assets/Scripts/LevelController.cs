using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Windows.Speech;
using UnityEngine.SceneManagement;
using UnityEngine.Tilemaps;

public class LevelController : MonoBehaviour
{
    private static string[] NUMBERS = new string[]
    {
        "zwei",
        "drei",
        "vier",
        "fünf",
        "sechs",
        "sieben",
        "acht",
        "neun"
    };

    private static string[] COMMANDS = new string[]
    {
        "hoch",
        "runter",
        "links",
        "rechts",
        "warten",
        "pause"
    };
    
    public delegate void TickEventHandler(string command);

    public Tilemap tilemap;
    public event TickEventHandler OnTick;

    private Queue<string> commands = new Queue<string>();
    private double timer = 0;
    private int multiply = 1;
    private PhraseRecognizer recognizer;

    void Awake()
    {
        string[] keywords = new string[COMMANDS.Length + NUMBERS.Length];
        COMMANDS.CopyTo(keywords, 0);
        NUMBERS.CopyTo(keywords, COMMANDS.Length);
        recognizer = new KeywordRecognizer(keywords, ConfidenceLevel.Low);
        recognizer.OnPhraseRecognized += OnRecognition;
    }

    void Update()
    {
        timer += Time.deltaTime;

        if (commands.Count > 0 && timer >= 1)
        {
            timer = 0;
            string command = commands.Dequeue();
            OnTick.Invoke(command);
        }
    }

    public Vector3 GridToWorldPos(Vector2Int grid)
    {
        int x = grid.x + tilemap.cellBounds.xMin;
        int y = grid.y + tilemap.cellBounds.yMin;
        Vector3 pos = tilemap.CellToWorld(new Vector3Int(x, y, 0));
        pos.z = 0;
        return pos;
    }

    public Vector2Int WorldToGridPos(Vector3 world)
    {
        Vector3Int grid = tilemap.WorldToCell(new Vector3(world.x, world.y, 0));
        int x = grid.x - tilemap.cellBounds.xMin;
        int y = grid.y - tilemap.cellBounds.yMin;
        return new Vector2Int(x, y);
    }

    public bool IsTileSolid(Vector2Int pos)
    {
        int x = pos.x + tilemap.cellBounds.xMin;
        int y = pos.y + tilemap.cellBounds.yMin;
        Tile tile = tilemap.GetTile<Tile>(new Vector3Int(x, y, 0));
        if (tile == null) return true;
        return tile.colliderType == Tile.ColliderType.Grid;
    }

    private int GetMultiplier(string command)
    {
        for (int i = 0; i < NUMBERS.Length; i++)
        {
            if (command.Equals(NUMBERS[i]))
            {
                return i + 2;
            }
        }
        return -1;
    }

    private void OnRecognition(PhraseRecognizedEventArgs args)
    {
        string command = args.text;
        Debug.Log(string.Format("Command: '{0}'", command));
        int multiply = GetMultiplier(command);

        if (command.Equals("pause"))
        {
            SceneManager.LoadScene("MainMenu");
        }
        else if (multiply != -1)
        {
            this.multiply = multiply;
        }
        else if (IsTickSource(command))
        {
            for (int i = 0; i < this.multiply; i++)
            {
                commands.Enqueue(command);
            }
            this.multiply = 1;
        }
    }

    private bool IsTickSource(string command)
    {
        return command.Equals("hoch") || command.Equals("runter") || command.Equals("links") || command.Equals("rechts") || command.Equals("warten");
    }

    void OnEnable() => recognizer.Start();
    void OnDisable() => recognizer.Stop();
    void OnDestroy() => recognizer.Dispose();
}
