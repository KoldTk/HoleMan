using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharInfo : MonoBehaviour
{
    public GridTile activeTile;
    public CharacterColor currentColor;
    private PathFinder _pathFinder;
    private GridTile _targetTile;
    private bool _canMove;
    private bool _isActive = true;
    private List<GridTile> _path = new List<GridTile>();
    [SerializeField] private float _moveSpeed;
    private void Update()
    {
    }
    private void LateUpdate()
    {
        if (!_isActive) return;
        if (_canMove && _path.Count > 0)
        {
            MoveAlongPath();
        }    
    }
    private void OnEnable()
    {
        _pathFinder = new PathFinder();
        EventDispatcher<CharacterColor>.AddListener(Event.HoleClick.ToString(), GetHoleColor);
        EventDispatcher<GridTile>.AddListener(Event.MoveCharacter.ToString(), ClickHole);

    }
    private void OnDisable()
    {
        EventDispatcher<CharacterColor>.RemoveListener(Event.HoleClick.ToString(), GetHoleColor);
        EventDispatcher<GridTile>.RemoveListener(Event.MoveCharacter.ToString(), ClickHole);
        
    }
    private void MoveAlongPath()
    {
        var step = _moveSpeed * Time.deltaTime;
        var zIndex = _path[0].transform.position.z;

        transform.position = Vector2.MoveTowards(transform.position, _path[0].transform.position, step);
        transform.position = new Vector3(transform.position.x, transform.position.y, zIndex);

        if (Vector2.Distance(transform.position, _path[0].transform.position) < 0.0001f)
        {
            _path.RemoveAt(0);
        }    
    }
    private void ClickHole(GridTile holeTile)
    {
        _targetTile = holeTile;
        _path = _pathFinder.FindPath(activeTile, _targetTile);
    }
    private void GetHoleColor(CharacterColor color)
    {
        if (currentColor != color) return;
        _canMove = true;
    } 
    private void OnReachDestination()
    {
        _isActive = false;
        gameObject.SetActive(false);
    }    
}
