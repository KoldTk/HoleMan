using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class PointHole : HoleNode
{
    [SerializeField] private int _counter = 16;
    [SerializeField] private UFOPointHoleArea _UFO;
    [SerializeField] private PointCaculateArea _pointCaculateArea;
    [SerializeField] private TextMeshProUGUI _counterText;
    private int _totalDrop = 0;
    public bool isInFront = false;

    private void Start()
    {
        ChangeHoleColor();
        _counterText.text = _counter.ToString();
    }

    private void OnEnable()
    {
        EventDispatcher<CharacterColor>.AddListener(Event.CountCharacter.ToString(), CountCharNum);
        EventDispatcher<CharacterColor>.AddListener(Event.SpawnUFO.ToString(), SpawnPointHoleUFO);
    }
    private void OnDisable()
    {
        EventDispatcher<CharacterColor>.RemoveListener(Event.CountCharacter.ToString(), CountCharNum);
        EventDispatcher<CharacterColor>.RemoveListener(Event.SpawnUFO.ToString(), SpawnPointHoleUFO);
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && isInFront)
        {
            _counter -= 1;
            _counterText.text = _counter.ToString();
            if (_counter <= 0)
            {
                RemovePointHole();
            }
        }
    }
    private void CountCharNum(CharacterColor charColor)
    {
        if (charColor == holeColor && isInFront)
        {
            _totalDrop++;
            _UFO.canSpawn = true;
        }
    }    
    private void SpawnPointHoleUFO(CharacterColor color)
    {
        if (!isInFront) return;
        if (_UFO.isActiveAndEnabled || !_UFO.canSpawn) return;
        StartCoroutine(SpawnUFOSequence());
    }
    private IEnumerator SpawnUFOSequence()
    {
        yield return new WaitForSeconds(0.5f);
        EventDispatcher<CharacterColor>.Dispatch(Event.SendCharToReserve.ToString(), _UFO.charToHoleColor);
        Debug.Log("DropNumber" + _UFO.dropNumber);
        if (_totalDrop > _counter)
        {
            int remainingChar = _totalDrop - _counter;
            _pointCaculateArea.SpawnReserve(remainingChar, _UFO.charToHoleColor);
            
            _UFO.dropNumber = _counter;
        }
        else
        {
            _UFO.dropNumber = _totalDrop;
        }
        yield return null;
        _totalDrop = 0;
        _UFO.gameObject.SetActive(true);
    }
    private void RemovePointHole()
    {
        //Animation sequence if have
        transform.parent.GetComponent<HoleColumn>().OnHoleDissapear(this);
        Destroy(gameObject, 0.2f);
    }  
}
