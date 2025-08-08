using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HoleTile : MonoBehaviour
{
    public CharacterColor holeColor;
    private Vector2Int _gridPos;

    private void Update()
    {
        ButtonClick();
    }
    private void Start()
    {

    }
    public void SetupPos(Vector2Int gridPos)
    {
        _gridPos = gridPos;
    }
    
    private void ButtonClick()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out RaycastHit hitInfo))
            {
                if (hitInfo.collider.CompareTag("Hole"))
                {
                    EventDispatcher<CharacterColor>.Dispatch(Event.HoleClick.ToString(), holeColor);
                }
            }
        }
    }    
}
