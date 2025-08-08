using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewHole", menuName = "Database/Create Hole")]
public class HoleDatabase : ScriptableObject
{
    public HoleInfo[] holeInfos;

    public HoleInfo this[int index] => holeInfos[index];
}

[System.Serializable]
public class HoleInfo
{
    public GameObject characterPrefab;
    public CharacterColor holeColor;
}
