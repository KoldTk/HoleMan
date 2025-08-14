using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReserveCharacter : MonoBehaviour
{
    private Transform _targetPoint;
    [SerializeField] private float _moveSpeed = 5f;

    private void Update()
    {
        if (_targetPoint != null)
        {
            transform.position = Vector3.MoveTowards(transform.position, _targetPoint.position, _moveSpeed * Time.deltaTime);

            if (Vector3.Distance(transform.position, _targetPoint.position) < 0.05f)
            {
                transform.position = _targetPoint.position;
            }
        }
    }
    private void OnDisable()
    {
        if (_targetPoint != null)
        {
            RemoveKinematic(gameObject);
        }
    }
    public void AssignToPoint(Transform point)
    {
        _targetPoint = point;
    }
    public void RemoveKinematic(GameObject character)
    {
        character.GetComponent<Rigidbody>().isKinematic = false;
    }
}
