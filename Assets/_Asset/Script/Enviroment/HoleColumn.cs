using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HoleColumn : MonoBehaviour
{
    public List<PointHole> holes = new List<PointHole>();
    [SerializeField] private float _moveSpeed = 5f;
    void Start()
    {
        foreach (Transform child in transform)
        {
            PointHole hole = child.GetComponent<PointHole>();
            if (hole != null)
            {
                holes.Add(hole);
            }    
        }
        if (holes.Count > 0)
        {
            holes[0].isInFront = true;
        }    
    }

    public void OnHoleDissapear(PointHole completeHole)
    {
        if (holes.Count == 0) return;
        Vector3 frontHole = holes[0].transform.position;
        if (holes[0] == completeHole)
        {
            holes.RemoveAt(0);
            Destroy(completeHole.gameObject);

            if (holes.Count > 0)
            {
                StartCoroutine(MoveToFront(frontHole, holes[0].transform));
                holes[0].isInFront = true;
            }    
        }
    }
    private IEnumerator MoveToFront(Vector3 frontPos, Transform behindHole)
    {
        while (Vector3.Distance(frontPos, behindHole.position) > 0)
        {
            behindHole.position = Vector3.MoveTowards(behindHole.position, frontPos, _moveSpeed * Time.deltaTime);
            yield return null;
        }    
    }    
}
