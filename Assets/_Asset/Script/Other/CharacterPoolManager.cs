using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.TextCore.Text;

public class CharacterPoolManager : Singleton<CharacterPoolManager>
{
    [SerializeField] private CharacterDatabase _characterDatabase;
    [SerializeField] private int _poolSizePerCharacter = 50;
    [SerializeField] private Transform _parentTransform;
    private Dictionary<int, Queue<GameObject>> _characterPool;
    private void Start()
    {
        PoolInitialize();
    }
    public void PoolInitialize()
    {
        _characterPool = new Dictionary<int, Queue<GameObject>>();
        for (int i = 0; i < _characterDatabase.characterInfos.Length; i++)
        {
            Queue<GameObject> poolList = new Queue<GameObject>();
            for (int j = 0; j < _poolSizePerCharacter; j++)
            {
                GameObject character = Instantiate(_characterDatabase.characterInfos[i].characterPrefab, _parentTransform);
                character.SetActive(false);
                poolList.Enqueue(character);
            }
            _characterPool.Add(i, poolList);
        }
    }
    public GameObject GetCharacter(int charID, Vector3 position, Quaternion rotation)
    {
        if (!_characterPool.ContainsKey(charID))
        {
            Debug.Log("Does not contain key");
            return null;
        }
        GameObject character = null;
        if (_characterPool[charID].Count > 0)
        {
            character = _characterPool[charID].Dequeue();
        }
        else
        {
            //Create more prefab if pool doens't have enough
            character = Instantiate(_characterDatabase.characterInfos[charID].characterPrefab, _parentTransform);
        }
        character.transform.SetPositionAndRotation(position, rotation);
        character.SetActive(true);
        return character;
    }
    public void ReturnToPool(int charID, GameObject character)
    {
        character.SetActive(false);
        _characterPool[charID].Enqueue(character);
    }    
}
