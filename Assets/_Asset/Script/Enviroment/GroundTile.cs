using DG.Tweening.Core.Easing;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroundTile : MonoBehaviour
{
    private Vector3 _originalPos;
    [SerializeField] private float _lowerAmount;
    [SerializeField] private float _lowerSpeed;
    [SerializeField] private float _detectRadius;
    [SerializeField] private float _detectDistance;
    private bool _isStepped = false;
    [SerializeField] private LayerMask _playerLayer;
    public int posX;
    public int posY;

    void Start()
    {
        _originalPos = transform.position;
    }
    void Update()
    {
        Vector3 targetPos;
        DetectPlayer();
        
        //Lower block if character step on, return to original when character leave
        if (_isStepped)
        {
            targetPos = _originalPos + Vector3.down * _lowerAmount;
        }
        else
        {
            targetPos = _originalPos;
        }
        Vector3 nextPos = Vector3.Lerp(transform.position, targetPos, Time.deltaTime * _lowerSpeed);
        MoveBlock(nextPos);
    }
    private void DetectPlayer()
    {
        //Detect player go through the block
        Vector3 origin = transform.position + Vector3.back * 0.1f;
        Vector3 direction = Vector3.back;
        _isStepped = Physics.SphereCast(origin, _detectRadius, direction, out RaycastHit hit, _detectDistance, _playerLayer);
    }   
    private void MoveBlock(Vector3 targetPos)
    {
        float minY = _originalPos.y - _lowerAmount;
        float maxY = _originalPos.y;
        targetPos.y = Mathf.Clamp(targetPos.y, minY, maxY);

        transform.position = targetPos;
    }    
    public void SetupTilePos(int x, int y)
    {
        posX = x;
        posY = y;
    }
}
