using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class MouseController : MonoBehaviour
{
    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            ButtonClick();
        }
    }
    private void ButtonClick()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        
        if (Physics.Raycast(ray, out RaycastHit hit, Mathf.Infinity, LayerMask.GetMask("Grid")))
        {
            if (hit.collider != null && hit.collider.CompareTag("Hole"))
            {
                var holeTile = hit.transform.GetComponent<Node>();
                var holeColor = hit.transform.GetComponent<HoleNode>();
                if (holeTile != null)
                {
                    EventDispatcher<Node>.Dispatch(Event.MoveCharacter.ToString(), holeTile);
                    EventDispatcher<CharacterColor>.Dispatch(Event.HoleClick.ToString(), holeColor.holeColor);
                }
            }
        }    
    } 
}
