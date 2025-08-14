using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UFOController : MonoBehaviour
{
    public Transform _dropPoint;
    public bool canSpawn;
    public readonly Dictionary<CharacterColor, Color> colorToID = new()
    {
        //Get color that match ID
        [CharacterColor.Red] = Color.red,
        [CharacterColor.Blue] = Color.blue,
        [CharacterColor.Yellow] = Color.yellow,
        [CharacterColor.Green] = Color.green,
    };
    private void OnDisable()
    {
        canSpawn = false;
    }
}
