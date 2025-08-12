using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Node : MonoBehaviour
{
    public float G; //Cost from starting node to target
    public float H; //Cost from end node to target
    public float F { get { return G + H; } } //Cost from starting to end
    public bool isBlocked;
    public bool hasCharacter;
    public Node previous;
    public Vector3Int nodePos;
    public CharacterColor gridColor;
}
