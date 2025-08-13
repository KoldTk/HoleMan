using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class UFOController : MonoBehaviour
{
    [SerializeField] private Transform _dropPoint;

    public int dropNumber;
    public CharacterColor characterColor;
    public bool canSpawn;
    private readonly Dictionary<CharacterColor, Color> _colorToID = new()
    {
        //Get color that match ID
        [CharacterColor.Red] = Color.red,
        [CharacterColor.Blue] = Color.blue,
        [CharacterColor.Yellow] = Color.yellow,
        [CharacterColor.Green] = Color.green,
    };
    private void OnEnable()
    {
        StartCoroutine(SpawnPlayer(dropNumber));
    }
    private void OnDisable()
    {
        canSpawn = false;
    }
    public IEnumerator SpawnPlayer(int spawnNum)
    {
        Color charColor = _colorToID.TryGetValue(characterColor, out Color value) ? value : Color.red;
        for (int i = 0; i < spawnNum; i++)
        {
            GameObject player = FallingCharPoolManager.Instance.GetCharacter(_dropPoint.position, _dropPoint.rotation);
            Renderer playerRenderer = player.GetComponentInChildren<Renderer>();
            playerRenderer.material.color = charColor;
            yield return new WaitForSeconds(0.1f);
        }
        gameObject.SetActive(false);
    } 
    
}
