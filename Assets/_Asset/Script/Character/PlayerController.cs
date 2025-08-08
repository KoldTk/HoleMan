using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public CharacterColor characterColor;
    private void OnEnable()
    {
        EventDispatcher<CharacterColor>.AddListener(Event.HoleClick.ToString(), MoveCharacter);
    }
    private void OnDisable()
    {
        EventDispatcher<CharacterColor>.RemoveListener(Event.HoleClick.ToString(), MoveCharacter);
    }
  
    private void MoveCharacter(CharacterColor color)
    {
        if (color == characterColor)
        {
            Debug.Log("Move red");
        }
    }    
}
