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
        Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector2 mousePos2d = new Vector2(mousePos.x, mousePos.y);
        RaycastHit2D hit = Physics2D.Raycast(mousePos2d, Vector2.zero, Mathf.Infinity, LayerMask.GetMask("Grid"));
        if (hit.collider != null && hit.collider.CompareTag("Hole"))
        {
            var holeTile = hit.transform.GetComponent<GridTile>();
            var holeColor = hit.transform.GetComponent<HoleTile>();
            if (holeTile != null)
            {
                EventDispatcher<GridTile>.Dispatch(Event.MoveCharacter.ToString(), holeTile);
                EventDispatcher<CharacterColor>.Dispatch(Event.HoleClick.ToString(), holeColor.holeColor);
            } 
        }
    } 
}
