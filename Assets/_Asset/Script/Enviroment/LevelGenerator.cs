using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;

public class LevelGenerator : Singleton<LevelGenerator>
{
    [SerializeField] private int _level;
    [SerializeField] private float _spacing;
    public Dictionary<Vector3Int, Node> map;
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
    public Node overlayTilePrefab;
    public Transform overlayContainer;

    void Start()
    {
        string levelFilePath = $"Levels/Level{_level}";
        map = new Dictionary<Vector3Int, Node>();
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


        int rows = lines.Length;
        int cols = lines[0].Trim().Split(' ').Length;


        float offsetX = (cols - 1) * _spacing / 2f;
        float offsetZ = (rows - 1) * _spacing / 2f;

        for (int row = 0; row < rows; row++)
        {
            string line = lines[row].Trim();
            if (string.IsNullOrEmpty(line))
                continue;

            string[] cells = line.Split(' ');
            for (int col = 0; col < cols; col++)
            {
                int cellCode = int.Parse(cells[col]);
                if (cellCode == 0) continue;

                float worldX = col * _spacing - offsetX;
                float worldZ = -(row * _spacing - offsetZ); 
                Vector3 spawnPos = parentPos + new Vector3(worldX, 0, worldZ);

                Vector3Int cellPos = new Vector3Int(col, 0, -row);

                if (cellCode < 0)
                {

                    bool isTopLeft =
                        (col == 0 || int.Parse(cells[col - 1]) != cellCode) &&
                        (row == 0 || int.Parse(lines[row - 1].Trim().Split(' ')[col]) != cellCode);

                    if (isTopLeft)
                    {
                        Vector3 holePos = spawnPos + new Vector3(_spacing / 2f, 0, -_spacing / 2f);
                        SpawnHole(cellCode, holePos);
                    }
                }
                else if (cellCode > 0)
                {
                    Instantiate(tilePrefab, spawnPos, Quaternion.identity, transform);
                }

                SpawnGridMap(cellCode, spawnPos);
            }
        }
    }
    private void SpawnHole(int cellCode, Vector3 spawnPos)
    {
        int ID = _cellCodeToID.TryGetValue(cellCode, out int value) ? value : 1;
        SetupHoleInfo(ID, spawnPos);
    }
    private void SpawnGridMap(int cellCode, Vector3 spawnPos)
    {
        int ID = _cellCodeToID.TryGetValue(cellCode, out int value) ? value : 1;
        Vector3 overlayPos = new Vector3(spawnPos.x, spawnPos.y + _spacing, spawnPos.z);

        Vector3Int nodePos = new Vector3Int(
            Mathf.FloorToInt(spawnPos.x / _spacing),
            Mathf.FloorToInt(spawnPos.y / _spacing),
            Mathf.FloorToInt(spawnPos.z / _spacing)
        );
        var overlayTile = Instantiate(overlayTilePrefab, overlayPos, Quaternion.identity, overlayContainer);
        if (cellCode < 0)
        {
            overlayTile.AddComponent<HoleNode>();
            HoleNode info = overlayTile.GetComponent<HoleNode>();
            info.holeColor = holeDatabase[ID].holeColor;
            info.tag = "Hole";
        }
        else
        {
            SetupCharacterInfo(ID, spawnPos, overlayTile, nodePos);
        }
        if (!map.ContainsKey(nodePos))
        {
            map.Add(nodePos, overlayTile);
        }
        else
        {
            Debug.LogWarning($"Duplicate nodePos: {nodePos}");
        }

        //Save node information
        overlayTile.nodePos = nodePos;
        overlayTile.gridColor = characterDatabase[ID].characterColor;
    }
    private void SetupCharacterInfo(int charID, Vector3 spawnPos, Node node, Vector3Int nodePos)
    {
        GameObject character = CharacterPoolManager.Instance.GetCharacter(characterDatabase[charID].ID, spawnPos + Vector3.up * 0.1f, Quaternion.identity);
        PlayerController info = character.GetComponent<PlayerController>();
        if (info != null)
        {
            info.characterColor = characterDatabase[charID].characterColor;
            info.activeTile = node;
            info.nodePos = nodePos;
        }
    }
    private void SetupHoleInfo(int holeID, Vector3 spawnPos)
    {
        GameObject hole = Instantiate(holePrefab, spawnPos, Quaternion.identity, transform);
        HoleNode info = hole.GetComponent<HoleNode>();
        info.holeColor = characterDatabase[holeID].characterColor;
    }
}
