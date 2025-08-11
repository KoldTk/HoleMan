using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PointCaculateArea : MonoBehaviour
{
    [SerializeField] private UFOController _UFO;
    [SerializeField] private PointHole _pointHole;
    [SerializeField] private GameObject _additionalArea;
    [SerializeField] private List<ReserveArea> _reserveArea = new List<ReserveArea>();
    private float _UFOStartingHeight;

    void Start()
    {
        _UFOStartingHeight = _UFO.transform.position.y;
    }
    private void OnEnable()
    {
        EventDispatcher<CharacterColor>.AddListener(Event.CaculatingPoint.ToString(), SpawnUFO);
        EventDispatcher<bool>.AddListener(Event.SpawnReserveArea.ToString(), SpawnAdditionalArea);
    }
    private void OnDisable()
    {
        EventDispatcher<CharacterColor>.RemoveListener(Event.CaculatingPoint.ToString(), SpawnUFO);
        EventDispatcher<bool>.RemoveListener(Event.SpawnReserveArea.ToString(), SpawnAdditionalArea);
    }

    private void SpawnUFO(CharacterColor color)
    {
        if (_pointHole._color == color)
        {
            _UFO.transform.position = new Vector3 (_pointHole.transform.position.x, _UFOStartingHeight, _pointHole.transform.position.z);
        } 
        else if ( _pointHole._color != color)
        {
            for (int i = 0; i < _reserveArea.Count; i++)
            {
                if (_reserveArea[i].areaColor == CharacterColor.None || _reserveArea[i].areaColor == color && _reserveArea[i].isActiveAndEnabled)
                {
                    _UFO.transform.position = new Vector3(_reserveArea[i].transform.position.x, _UFOStartingHeight, _reserveArea[i].transform.position.z);
                    _reserveArea[i].areaColor = color;
                    break;
                }    
            }    
        }
        _UFO.gameObject.SetActive(true);
    }
    private void SpawnAdditionalArea(bool isSpawned)
    {
        _additionalArea.SetActive(isSpawned);
    }    
}
