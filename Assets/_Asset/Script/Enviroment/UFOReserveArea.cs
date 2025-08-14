using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UFOReserveArea : UFOController
{
    public int charToReserveNum;
    public CharacterColor charToReserveColor;
    [SerializeField] private ReserveArea _reserveArea;
    [SerializeField] private FormationManager _formationManager;
    private void OnEnable()
    {
        StartCoroutine(SpawnToReserve(charToReserveNum));
    }
    private void OnDisable()
    {
        charToReserveNum = 0;
    }
    public IEnumerator SpawnToReserve(int spawnNum)
    {
        Color charColor = colorToID.TryGetValue(charToReserveColor, out Color value) ? value : Color.red;
        for (int i = 0; i < spawnNum; i++)
        {
            GameObject player = FallingCharPoolManager.Instance.GetCharacter(_dropPoint.position, _dropPoint.rotation);
            Transform unoccupiedPoint = _formationManager.GetEmptyPoint();
            player.AddComponent<ReserveCharacter>();
            player.GetComponent<ReserveCharacter>().AssignToPoint(unoccupiedPoint);
            Renderer playerRenderer = player.GetComponentInChildren<Renderer>();
            playerRenderer.material = new Material(playerRenderer.material);
            playerRenderer.material.color = charColor;
            SetKinematic(player);
            SetColorToArea(charToReserveColor);
            yield return new WaitForSeconds(0.3f);
        }
        
        gameObject.SetActive(false);
    }
    public void SetColorToArea(CharacterColor color)
    {
        if (_reserveArea.areaColor == CharacterColor.None)
        {
            _reserveArea.areaColor = charToReserveColor;
        }
    }
    private void SetKinematic(GameObject character)
    {
        character.GetComponent<Rigidbody>().isKinematic = true;
    }   
}
