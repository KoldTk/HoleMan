using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PointHole : MonoBehaviour
{
    [SerializeField] private int _counter = 16;
    public CharacterColor _color;
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            _counter -= 1;
            
            if (_counter <= 0)
            {
                Destroy(gameObject);
            }    
        }
    } 
}
