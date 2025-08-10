using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class CharInfo : MonoBehaviour
{
    public GridTile activeTile;
    public CharacterColor characterColor;
    public int characterID;
    private PathFinder _pathFinder;
    private GridTile _targetTile;
    private bool _canMove;
    private List<GridTile> _path = new List<GridTile>();
    [SerializeField] private float _moveSpeed;
    private void LateUpdate()
    {
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
        CharacterPoolManager.Instance.ReturnToPool(characterID, gameObject);
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
            if (_path.Count == 0)
            {
                activeTile.gridColor = CharacterColor.None;
                activeTile.isBlocked = false;
                _canMove = false;
                gameObject.SetActive(false);
            }    
        }
    }
    private void ClickHole(GridTile holeTile)
    {
        _targetTile = holeTile;
        TryFindPathAndMove();
        Debug.Log(_canMove);
    }
    private void GetHoleColor(CharacterColor color)
    {
        if (characterColor != color) return;
        _canMove = true;
        TryFindPathAndMove();
    }
    private void TryFindPathAndMove()
    {
        if (_targetTile == null || !_canMove) return;

        _path = _pathFinder.FindPath(activeTile, _targetTile);
        Debug.Log($"FindPath returned path with {_path.Count} nodes");
        if (_path.Count == 0)
        {
            Debug.LogWarning("No path found!");
            _canMove = false; // không di chuyển nếu không có đường
        }
    }
}
