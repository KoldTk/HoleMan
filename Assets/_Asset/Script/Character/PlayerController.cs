using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class PlayerController : MonoBehaviour
{
    public Node activeTile;
    public Vector3Int nodePos;
    public CharacterColor characterColor;
    public int characterID;
    private PathFinder _pathFinder;
    private Node _targetTile;
    private bool _canMove;
    private bool _isMoving;
    private float _fixedY;
    private bool _lockY = false;
    private bool _isJumping = false;
    private List<Node> _path = new List<Node>();
    [SerializeField] private float _moveSpeed;
    [SerializeField] private float _jumpForce;
    //[SerializeField] Animator _animator;
    [SerializeField] Rigidbody _rb;
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
        EventDispatcher<Node>.AddListener(Event.MoveCharacter.ToString(), ClickHole);

    }
    private void OnDisable()
    {
        EventDispatcher<CharacterColor>.RemoveListener(Event.HoleClick.ToString(), GetHoleColor);
        EventDispatcher<Node>.RemoveListener(Event.MoveCharacter.ToString(), ClickHole);
        
    }
    private void OnTriggerEnter(Collider other)
    {
        HoleNode hole = other.GetComponent<HoleNode>();
        if (other.CompareTag("Hole") && hole.holeColor == characterColor)
        {
            EventDispatcher<bool>.Dispatch(Event.SpawnUFO.ToString(), true);
            CharacterPoolManager.Instance.ReturnToPool(characterID, gameObject);
        }    
    }
    private void MoveAlongPath()
    {
        Node targetNode = _path[0];
        var step = _moveSpeed * Time.deltaTime;
        if (_lockY == false)
        {
            _fixedY = transform.position.y; // Save current height
            _lockY = true;
        }
        //Move to hole
        Vector3 startPos = transform.position;
        Vector3 targetPos = new Vector3(_path[0].transform.position.x, _fixedY, _path[0].transform.position.z);
        transform.position = Vector3.MoveTowards(startPos, targetPos, step);
        Vector2 currentXZ = new Vector2(transform.position.x, transform.position.z);
        Vector2 targetXZ = new Vector2(targetPos.x, targetPos.z);
        //Jump if close to hole
        if (targetNode.GetComponent<HoleNode>() && !_isJumping && targetNode.gridColor == characterColor)
        {
            _isJumping = true;
            _rb.isKinematic = false;
            _rb.velocity = new Vector3(_rb.velocity.x, 0, _rb.velocity.z);
            _rb.AddForce(Vector3.up * _jumpForce, ForceMode.VelocityChange);
            //_animator.SetTrigger("Jump");
        }    
        if (Vector2.Distance(currentXZ, targetXZ) < 0.0001f)
        {
            _path.RemoveAt(0);
            _lockY = false;
            if (_path.Count == 0)
            {
                activeTile.isBlocked = false;
                _canMove = false;
            }
        }
    }
    private void ClickHole(Node holeTile)
    {
        if (_isMoving) return;
        _targetTile = holeTile;
        TryFindPathAndMove();
    }
    private void GetHoleColor(CharacterColor color)
    {
        if (characterColor != color) return;
        _canMove = true;
        TryFindPathAndMove();
    }
    private void TryFindPathAndMove()
    {
        if (_targetTile == null || !_canMove || _isMoving) return;

        _path = _pathFinder.FindPath(activeTile, _targetTile);
        Debug.Log($"FindPath returned path with {_path.Count} nodes");
        if (_path.Count > 0)
        {
            activeTile.gridColor = CharacterColor.None;
            _isMoving = true;
            EventDispatcher<CharacterColor>.Dispatch(Event.CountCharacter.ToString(), characterColor);
        }
        else
        {
            Debug.LogWarning("No path found!");
            _canMove = false;
        }
    }
}
