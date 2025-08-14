using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PointCaculateArea : MonoBehaviour
{
    [SerializeField] private GameObject _additionalArea;
    [SerializeField] private List<ReserveArea> _reserveArea = new List<ReserveArea>();
    private void OnEnable()
    {
        EventDispatcher<bool>.AddListener(Event.SpawnReserveArea.ToString(), SpawnAdditionalArea);
    }
    private void OnDisable()
    {
        EventDispatcher<bool>.RemoveListener(Event.SpawnReserveArea.ToString(), SpawnAdditionalArea);
    }

    public void SpawnReserve(int remainingChar, CharacterColor charColor)
    {
        for (int i = 0; i < _reserveArea.Count; i++)
        {
            if (_reserveArea[i].isActiveAndEnabled)
            {
                bool isValidArea = _reserveArea[i].IsValidArea();
                if (isValidArea)
                {
                    var UFOReserve = _reserveArea[i].UFO.GetComponent<UFOReserveArea>();
                    if (UFOReserve != null)
                    {
                        UFOReserve.charToReserveNum = remainingChar;
                        UFOReserve.charToReserveColor = charColor;
                        _reserveArea[i].ChangeAreaColor(charColor);
                    }    
                    _reserveArea[i].UFO.SetActive(true);
                    break;
                }
            }
        }
    }
    private void SpawnAdditionalArea(bool isSpawned)
    {
        _additionalArea.SetActive(isSpawned);
    }  
}
