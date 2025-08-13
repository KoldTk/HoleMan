using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Rendering;

public class HoleNode : MonoBehaviour
{
    public CharacterColor holeColor;
    private readonly Dictionary<CharacterColor, Color> _IDToColor = new()
    {
        //Get color that match ID
        [CharacterColor.Red] = Color.red,
        [CharacterColor.Blue] = Color.blue,
        [CharacterColor.Yellow] = Color.yellow,
        [CharacterColor.Green] = Color.green,
    };
    [SerializeField] private Renderer _holeBorder;

    public void ChangeHoleColor()
    {
        Color color = _IDToColor.TryGetValue(holeColor, out Color value) ? value : Color.red;
        _holeBorder.material.color = color;
    }    
}
