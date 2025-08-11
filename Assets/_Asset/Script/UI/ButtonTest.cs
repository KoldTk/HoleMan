using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonTest : MonoBehaviour
{
    public CharacterColor color;
    public void Button()
    {
        EventDispatcher<CharacterColor>.Dispatch(Event.CaculatePoint.ToString(), color);
    }
    public void SpawnReserveArea()
    {
        
        EventDispatcher<bool>.Dispatch(Event.SpawnReserveArea.ToString(), true);
    }    
}
