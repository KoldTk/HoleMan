using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReserveArea : MonoBehaviour
{
    public CharacterColor areaColor = CharacterColor.None;
    void Start()
    {
        
    }
    private void OnCollisionEnter(Collision collision)
    {
        Rigidbody playerBody = collision.gameObject.GetComponent<Rigidbody>();
        playerBody.isKinematic = true;
    }
}
