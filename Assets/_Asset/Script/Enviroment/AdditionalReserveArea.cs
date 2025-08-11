using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class AdditionalReserveArea : MonoBehaviour
{
    [SerializeField] private Transform _reserveArea;
    private float _spacing = 5f;
    private float _moveSpeed = 5;

    private void OnEnable()
    {
        StartCoroutine(MoveReserverArea());
    }
    private IEnumerator MoveReserverArea()
    {
        while (Vector3.Distance(transform.position, _reserveArea.position) <= _spacing)
        {
            _reserveArea.Translate(Vector3.left * _moveSpeed * Time.deltaTime);
            yield return null;
        }    
    }    
}
