using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FormationManager : MonoBehaviour
{
    public static FormationManager Instance;
    private List<Transform> _points = new List<Transform>();
    private List<Transform> _usedPoints = new List<Transform>();

    private void Awake()
    {
        Instance = this;
        foreach (Transform child in transform)
        {
            _points.Add(child);
        }
    }

    public Transform GetEmptyPoint()
    {
        foreach (var point in _points)
        {
            if (!_usedPoints.Contains(point))
            {
                _usedPoints.Add(point);
                return point;
            }
        }
        // All positions are occupied
        return null; 
    }

    public void ReleasePoint(Transform point)
    {
        // Return to empty point when character leave
        if (_usedPoints.Contains(point))
        {
            _usedPoints.Remove(point);
        }
    }
}
