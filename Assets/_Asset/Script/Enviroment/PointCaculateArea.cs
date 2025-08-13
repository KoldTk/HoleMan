using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PointCaculateArea : MonoBehaviour
{
    [SerializeField] private PointHole _pointHole;
    [SerializeField] private GameObject _additionalArea;
    [SerializeField] private List<ReserveArea> _reserveArea = new List<ReserveArea>();
    void Start()
    {

    }
    private void OnEnable()
    {
        EventDispatcher<CharacterColor>.AddListener(Event.CaculatePoint.ToString(), SpawnUFO);
        EventDispatcher<bool>.AddListener(Event.SpawnReserveArea.ToString(), SpawnAdditionalArea);
    }
    private void OnDisable()
    {
        EventDispatcher<CharacterColor>.RemoveListener(Event.CaculatePoint.ToString(), SpawnUFO);
        EventDispatcher<bool>.RemoveListener(Event.SpawnReserveArea.ToString(), SpawnAdditionalArea);
    }

    private void SpawnUFO(CharacterColor color)
    {
        //if (_pointHole._color == color)
        //{
           
        //} 
        //else if ( _pointHole._color != color)
        //{
        //    for (int i = 0; i < _reserveArea.Count; i++)
        //    {
        //        if (_reserveArea[i].areaColor == CharacterColor.None || _reserveArea[i].areaColor == color && _reserveArea[i].isActiveAndEnabled)
        //        {
                    
        //            _reserveArea[i].areaColor = color;
        //            break;
        //        }    
        //    }    
        //}
    }
    private void SpawnAdditionalArea(bool isSpawned)
    {
        _additionalArea.SetActive(isSpawned);
    }    
}
