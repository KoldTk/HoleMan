using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class FormationManager : MonoBehaviour
{
    public static FormationManager Instance;
    private int _counter = 32;
    private List<Transform> _points = new List<Transform>();
    private List<Transform> _usedPoints = new List<Transform>();
    [SerializeField] private TextMeshProUGUI _counterText;

    private void Awake()
    {
        Instance = this;
        foreach (Transform child in transform)
        {
            _points.Add(child);
        }
    }
    private void Start()
    {
        _counterText.text = _counter.ToString();
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
            _counterText.text = $"{_counter - _usedPoints.Count}";
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
        _counterText.text = $"{_counter - _usedPoints.Count}";
    }
}
