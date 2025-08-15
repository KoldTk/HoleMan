using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PointHoleArea : MonoBehaviour
{
    private List<PointHole> pointHoles = new List<PointHole>();
    // Start is called before the first frame update
    void Start()
    {
        pointHoles.AddRange(transform.GetComponentsInChildren<PointHole>());
        var tempList = new List<PointHole>(pointHoles);
        foreach (PointHole pointHole in tempList)
        {
            pointHoles.Add(pointHole);
        }
    }
    // Update is called once per frame
    void Update()
    {
        if (pointHoles.Count == 0)
        {
            //Stage Clear
        }    
    }
}
