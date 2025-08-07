using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewPlayer", menuName = "Database/Create Character")]   
public class CharacterDatabase : ScriptableObject
{
    public CharacterInfo[] characterInfos;
    public CharacterInfo this[int index] => characterInfos[index];
}

[System.Serializable]
public class CharacterInfo
{
    public string characterName;
    public GameObject characterPrefab;
    public CharacterColor characterColor;
}
public enum CharacterColor { Red, Green, Blue}
