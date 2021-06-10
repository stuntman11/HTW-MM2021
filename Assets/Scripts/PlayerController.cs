using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class PlayerController : MonoBehaviour
{
    private float timer = 0;
    public Tilemap tilemap;

    private Vector2Int currentPos;
    private Vector2Int lastPos;

    public PlayerController()
    {
        this.currentPos = new Vector2Int();
        this.lastPos = currentPos;
    }

    void Awake()
    {
        LevelController level = GameObject.Find("Controller").GetComponent<LevelController>();
        level.OnTick += OnTick;
    }

    void Update()
    {
        timer += Time.deltaTime;
        Vector3 current = GridToWorldPos(currentPos);
        Vector3 last = GridToWorldPos(lastPos);
        transform.position = Vector3.Lerp(last, current, timer * 5);
    }

    private void OnTick(string command)
    {
        this.timer = 0;
        this.lastPos = currentPos;

        if (command.Equals("hoch")) this.currentPos.y += 1;
        else if (command.Equals("runter")) this.currentPos.y -= 1;
        else if (command.Equals("links")) this.currentPos.x -= 1;
        else if (command.Equals("rechts")) this.currentPos.x += 1;
    }

    private Vector3 GridToWorldPos(Vector2Int grid)
    {
        int x = grid.x + tilemap.cellBounds.xMin;
        int y = grid.y + tilemap.cellBounds.yMin;
        Vector3 pos = tilemap.CellToWorld(new Vector3Int(x, y, 0));
        pos.z = transform.position.z;
        return pos;
    }
}
