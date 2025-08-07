using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelGenerator : MonoBehaviour
{
    [SerializeField] private int level;
    [SerializeField] private float spacing = 1.1f;
    public GameObject tilePrefab;
    public CharacterDatabase characterDatabase;
    public GameObject holePrefab;
    
    // 0 = empty, 1 = red, 2 = blue, 3 = hole
    void Start()
    {
        string levelFilePath = $"Levels/Level{level}";
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
        string[] lines = textFile.text.Split('\n');
        for (int y = 0; y < lines.Length; y++)
        {
            string line = lines[y].Trim();
            if (string.IsNullOrEmpty(line))
                continue;

            string[] cells = line.Split(' ');
            for (int x = 0; x < cells.Length; x++)
            {
                int cellCode = int.Parse(cells[x]);
                Vector3 spawnPos = new Vector3(x * spacing, -y * spacing, 0);

                if (cellCode == 3)
                {
                    //Check if top and left of spawn position is empty
                    bool isTopLeft =
                         (x == 0 || cells[x - 1] != "3") &&
                         (y == 0 || lines[y - 1].Trim().Split(' ')[x] != "3");
                    if (isTopLeft)
                    {
                        Instantiate(holePrefab, spawnPos, Quaternion.identity, transform);
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
    private void SpawnCharacter(int cellCode, Vector3 spawnPos)
    {
        switch (cellCode)
        {
            case 1:
                Instantiate(characterDatabase[0].characterPrefab, spawnPos + Vector3.back, Quaternion.identity);
                break;
            case 2:
                Instantiate(characterDatabase[1].characterPrefab, spawnPos + Vector3.back, Quaternion.identity);
                break;
            default:
                break;
        }
    }    
}
