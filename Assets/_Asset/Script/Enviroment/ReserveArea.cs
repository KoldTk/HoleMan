using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReserveArea : MonoBehaviour
{
    public CharacterColor areaColor = CharacterColor.None;
    public GameObject UFO;
    public SpriteRenderer areaSprite;
    public CharacterColor charDropColor;
    private readonly Dictionary<CharacterColor, Color> _colorToID = new()
    {
        //Get color that match ID
        [CharacterColor.Red] = Color.red,
        [CharacterColor.Blue] = Color.blue,
        [CharacterColor.Yellow] = Color.yellow,
        [CharacterColor.Green] = Color.green,
    };
    public List<ReserveCharacter> reserveCharList = new List<ReserveCharacter>();
    private void OnEnable()
    {
        EventDispatcher<CharacterColor>.AddListener(Event.SendCharToReserve.ToString(), GetCharColor);
    }
    private void OnDisable()
    {
        EventDispatcher<CharacterColor>.RemoveListener(Event.SendCharToReserve.ToString(), GetCharColor);
    }
    public bool IsValidArea()
    {
        if(areaColor == CharacterColor.None || areaColor == charDropColor)
        {
            return true;
        }    
        return false;
    }
    public void GetCharColor(CharacterColor color)
    {
        charDropColor = color;
    }
    
    public void ChangeAreaColor(CharacterColor charColor)
    {
        Color areaColor = _colorToID.TryGetValue(charColor, out Color value) ? value : Color.red;
        areaSprite.color = areaColor;
    }   
}
