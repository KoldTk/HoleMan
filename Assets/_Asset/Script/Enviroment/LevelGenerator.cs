using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class LevelGenerator : Singleton<LevelGenerator>
{
    [SerializeField] private int _level;
    [SerializeField] private float _spacing;
    public Dictionary<Vector2, GridTile> map;
    private readonly Dictionary<int, int> _cellCodeToID = new()
    {
        //Get prefabID that match the cell code
        //1 = red, 2 = blue, 3 = yellow, 4 = purple
        //-1 = red hole, -2 = blue hole, -3 = yellow hole, -4 = purple hole
        [1] = 0, [-1] = 0,
        [2] = 1, [-2] = 1,
        [3] = 2, [-3] = 2,
        [4] = 3, [-4] = 3,
    };
    [Header("Database")]
    public CharacterDatabase characterDatabase;
    public HoleDatabase holeDatabase;

    [Header("Prefabs")]
    public GameObject tilePrefab;
    public GameObject holePrefab;
    public GridTile overlayTilePrefab;
    public GameObject overlayContainer;

    void Start()
    {
        string levelFilePath = $"Levels/Level{_level}";
        map = new Dictionary<Vector2, GridTile>();
        GenerateLevel(levelFilePath);
    }
    void GenerateLevel(string filePath)
    {
        TextAsset textFile = Resources.Load<TextAsset>(filePath);
        if (textFile == null)
        {
            Debug.Log("File not found");
            return;
        }
        SpawnTile(textFile);
    }
    private void SpawnTile(TextAsset textFile)
    {
        Vector3 parentPos = transform.position;
        string[] lines = textFile.text.Split('\n');

        //Get offset value so tile spawn map around the center
        float offsetX = GetOffsetX(lines);
        float offsetY = GetOffsetY(lines);

        for (int y = 0; y < lines.Length; y++)
        {
            string line = lines[y].Trim();
            if (string.IsNullOrEmpty(line))
                continue;

            //Spawn prefab depend on cell codes
            string[] cells = line.Split(' ');
            for (int x = 0; x < cells.Length; x++)
            {
                //Get spawn position for tile
                Vector3 spawnPos = parentPos + new Vector3(x * _spacing - offsetX, -y * _spacing + offsetY, 0);
                int cellCode = int.Parse(cells[x]); // Get cell codes to spawn the correct prefab
                Vector3Int cellPos = new Vector3Int(x, -y, 0);
                if (cellCode == 0) continue;
                if (cellCode < 0)
                {
                    //Check if top and left position is empty to spawn hole
                    bool isTopLeft =
                         (x == cellCode || cells[x - 1] != $"{cellCode}") &&
                         (y == cellCode || lines[y - 1].Trim().Split(' ')[x] != $"{cellCode}");
                    if (isTopLeft)
                    {
                        SpawnHole(cellCode, spawnPos);
                    }
                }
                else if (cellCode > 0)
                {
                    Instantiate(tilePrefab, spawnPos, Quaternion.identity, transform);
                }
                SpawnGridMap(cellCode, spawnPos); //Show map on 2D world
            }
        }
    }
    private float GetOffsetX(string[] lines)
    {
        int cols = lines[0].Trim().Split(' ').Length;
        float offsetX = (cols - 1) * _spacing / 2f;
        return offsetX;
    }
    private float GetOffsetY(string[] lines)
    {
        int rows = lines.Length;
        float offsetY = (rows - 1) * _spacing / 2f;
        return offsetY;
    }
    private void SpawnHole(int cellCode, Vector3 spawnPos)
    {
        int ID = _cellCodeToID.TryGetValue(cellCode, out int value) ? value : 1;
        SetupHoleInfo(ID, spawnPos);
    }
    private void SpawnGridMap(int cellCode, Vector3 spawnPos)
    {
        int ID = _cellCodeToID.TryGetValue(cellCode, out int value) ? value : 1;
        Vector3 overlayPos = new Vector3(spawnPos.x, spawnPos.y, spawnPos.z - 0.5f);
        var overlayKey = new Vector2(spawnPos.x, spawnPos.y);
        var overlayTile = Instantiate(overlayTilePrefab, overlayPos + Vector3.back, Quaternion.identity);
        if (cellCode < 0)
        {
            overlayTile.AddComponent<HoleTile>();
            HoleTile info = overlayTile.GetComponent<HoleTile>();
            info.holeColor = holeDatabase[ID].holeColor;
            info.tag = "Hole";
        }
        else
        {
            SetupCharacterInfo(ID, spawnPos, overlayTile);
        }
        map.Add(overlayKey, overlayTile);
        overlayTile.gridLocation = overlayPos;
        overlayTile.gridColor = characterDatabase[ID].characterColor;
    }
    private void SetupCharacterInfo(int charID, Vector3 spawnPos, GridTile tile)
    {
        GameObject character = CharacterPoolManager.Instance.GetCharacter(characterDatabase[charID].ID, spawnPos + Vector3.back, Quaternion.identity);
        CharInfo info = character.GetComponent<CharInfo>();
        info.characterColor = characterDatabase[charID].characterColor;
        info.activeTile = tile;
    }
    private void SetupHoleInfo(int holeID, Vector3 spawnPos)
    {
        GameObject hole = Instantiate(holePrefab, spawnPos, Quaternion.identity, transform);
        HoleTile info = hole.GetComponentInChildren<HoleTile>();
        info.holeColor = holeDatabase[holeID].holeColor;
    }    
}
