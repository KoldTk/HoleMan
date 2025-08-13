using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.TextCore.Text;

public class FallingCharacter : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Hole"))
        {
            FallingCharPoolManager.Instance.ReturnToPool(gameObject);
        }
    }
}
