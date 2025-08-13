using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PointHole : HoleNode
{
    [SerializeField] private int _counter = 16;
    [SerializeField] private UFOController _UFO;
    private int _totalDrop = 0;
    private bool _isInFront;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            _counter -= 1;
            
            if (_counter <= 0)
            {
                RemovePointHole();
            }    
        }
    }
    private void OnEnable()
    {
        EventDispatcher<CharacterColor>.AddListener(Event.CountCharacter.ToString(), CountCharNum);
        EventDispatcher<bool>.AddListener(Event.SpawnUFO.ToString(), SpawnUFO);
    }
    private void OnDisable()
    {
        EventDispatcher<CharacterColor>.RemoveListener(Event.CountCharacter.ToString(), CountCharNum);
        EventDispatcher<bool>.RemoveListener(Event.SpawnUFO.ToString(), SpawnUFO);
    }
    private void Start()
    {
        ChangeHoleColor();
    }
    private void CountCharNum(CharacterColor charColor)
    {
        if (charColor == holeColor)
        {
            _totalDrop++;
            _UFO.canSpawn = true;
        }
    }    
    private void SpawnUFO(bool isSpawned)
    {
        if (_UFO.isActiveAndEnabled || !_UFO.canSpawn) return;
        _UFO.characterColor = holeColor;
        StartCoroutine(SpawnUFOSequence());
    }
    private IEnumerator SpawnUFOSequence()
    {
        yield return new WaitForSeconds(0.5f);
        if (_totalDrop > _counter)
        {
            int remainingChar = _totalDrop - _counter;
            EventDispatcher<int>.Dispatch(Event.SendCharToReserve.ToString(), remainingChar);
            _UFO.dropNumber = _counter;
        }
        else
        {
            _UFO.dropNumber = _totalDrop;
        }
        yield return null;
        _UFO.gameObject.SetActive(true);
    }
    private void RemovePointHole()
    {
        //Animation sequence if have
        Destroy(gameObject, 0.2f);
    }    
}
