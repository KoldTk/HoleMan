using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelGenerator : MonoBehaviour
{
    [SerializeField] private int _level;
    [SerializeField] private float _spacing;
    public GameObject tilePrefab;
    public CharacterDatabase characterDatabase;
    public HoleDatabase holeDatabase;
    public GameObject holePrefab;
    private readonly Dictionary<int, int> _cellCodeToID = new()
    {
        //Get prefabID that match the cell code
        [1] = 0, [-1] = 0,
        [2] = 1, [-2] = 1,
        [3] = 2, [-3] = 2,
        [4] = 3, [-4] = 3,
    };    
    //1 = red, 2 = blue, 3 = yellow, 4 = purple
    //9 = red hole, 8 = blue hole, 7 = yellow hole, 6 = purple hole
    void Start()
    {
        string levelFilePath = $"Levels/Level{_level}";
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

        //Get offset value so tile spawn around the center
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
                else
                {
                    Instantiate(tilePrefab, spawnPos, Quaternion.identity, transform);
                }
                    SpawnCharacter(cellCode, spawnPos);
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
    private void SpawnCharacter(int cellCode, Vector3 spawnPos)
    {
        int ID = _cellCodeToID.TryGetValue(cellCode, out int value) ? value : 1;
        SetupCharacterInfo(ID, spawnPos);
    }
    private void SpawnHole(int cellCode, Vector3 spawnPos)
    {
        int ID = _cellCodeToID.TryGetValue(cellCode, out int value) ? value : 1;
        SetupHoleInfo(ID, spawnPos);
    }    
    private void SetupCharacterInfo(int charID, Vector3 spawnPos)
    {
        GameObject character = Instantiate(characterDatabase[charID].characterPrefab, spawnPos + Vector3.back, Quaternion.identity);
        PlayerController info = character.GetComponent<PlayerController>();
        info.characterColor = characterDatabase[charID].characterColor;
    }
    private void SetupHoleInfo(int holeID, Vector3 spawnPos)
    {
        GameObject hole = Instantiate(holePrefab, spawnPos, Quaternion.identity, transform);
        HoleTile info = hole.GetComponentInChildren<HoleTile>();
        info.holeColor = holeDatabase[holeID].holeColor;
    }    
    
}
