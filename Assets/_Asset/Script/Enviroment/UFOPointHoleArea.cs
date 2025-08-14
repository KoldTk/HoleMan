using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class UFOPointHoleArea : UFOController
{
    public int dropNumber;
    public CharacterColor charToHoleColor;
    private void OnEnable()
    {
        StartCoroutine(SpawnPlayer(dropNumber, _dropPoint));
    }
    private void OnDisable()
    {
        canSpawn = false;
        dropNumber = 0;
    }
    public IEnumerator SpawnPlayer(int spawnNum, Transform dropPoint)
    {
        Color charColor = colorToID.TryGetValue(charToHoleColor, out Color value) ? value : Color.red;
        for (int i = 0; i < spawnNum; i++)
        {
            GameObject player = FallingCharPoolManager.Instance.GetCharacter(dropPoint.position, dropPoint.rotation);
            Renderer playerRenderer = player.GetComponentInChildren<Renderer>();
            playerRenderer.material = new Material(playerRenderer.material);
            playerRenderer.material.color = charColor;
            yield return new WaitForSeconds(0.1f);
        }
        gameObject.SetActive(false);
    } 
    
}
