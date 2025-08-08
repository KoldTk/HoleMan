using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovementManager : Singleton<MovementManager>
{
    public TextAsset levelData;
    public PlayerController player;

    private int[,] _levelMap;
    private PathFinding _pathFinding;

    private void Start()
    {
        EventDispatcher<Vector2Int>.AddListener(Event.HoleClick.ToString(), MoveCharacter);
    }
    private void OnDisable()
    {
        EventDispatcher<Vector2Int>.RemoveListener(Event.HoleClick.ToString(), MoveCharacter);
    }
    
    public void MoveCharacter(Vector2Int targetPos)
    {
        
    }    
}
