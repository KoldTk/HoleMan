using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UFOController : MonoBehaviour
{
    [SerializeField] private GameObject playerPrefab;
    private void OnEnable()
    {
        StartCoroutine(SpawnPlayer(16));
    }

    public IEnumerator SpawnPlayer(int spawnNum)
    {
        for (int i = 0; i < spawnNum; i++)
        {
            GameObject player = Instantiate(playerPrefab, transform.position, Quaternion.identity);
            Rigidbody playerBody = player.GetComponentInChildren<Rigidbody>();
            playerBody.isKinematic = false;
            yield return new WaitForSeconds(0.5f);
        }
        gameObject.SetActive(false);
    }    
}
