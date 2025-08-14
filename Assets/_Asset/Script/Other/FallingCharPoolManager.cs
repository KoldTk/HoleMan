using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FallingCharPoolManager : Singleton<FallingCharPoolManager>
{
    [SerializeField] private GameObject _fallingcharPrefab;
    [SerializeField] private int _poolSizePerCharacter = 50;
    [SerializeField] private Transform _parentTransform;
    private Queue<GameObject> _fallingCharQueue = new Queue<GameObject>();
    
    void Start()
    {
        PoolInitialize();
    }
    public void PoolInitialize()
    {
        for (int i = 0; i < _poolSizePerCharacter; i++)
        {
            GameObject character = Instantiate(_fallingcharPrefab, _parentTransform);
            character.SetActive(false);
            _fallingCharQueue.Enqueue(character);
        }    
    }
    public GameObject GetCharacter(Vector3 position, Quaternion rotation)
    {
        GameObject character = null;
        if (_fallingCharQueue.Count > 0)
        {
            character = _fallingCharQueue.Dequeue();
        }
        else
        {
            character = Instantiate(_fallingcharPrefab, _parentTransform);
        }
        character.transform.SetPositionAndRotation(position, rotation);
        character.SetActive(true);
        return character;
    }
    public void ReturnToPool(GameObject character)
    {
        var reserveCharComp = character.GetComponent<ReserveCharacter>();
        if (reserveCharComp != null)
        {
            Destroy(reserveCharComp);
        }
        character.SetActive(false);
        _fallingCharQueue.Enqueue(character);
    }    
}
