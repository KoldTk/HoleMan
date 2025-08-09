using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridTile : MonoBehaviour
{
    public float G; //Distance from starting node
    public float H; //Distance from end node
    public float F { get { return G + H; } }
    public bool isBlocked;
    public bool hasCharacter;
    public GridTile previous;
    public Vector3 gridLocation;
    public CharacterColor gridColor;
}
