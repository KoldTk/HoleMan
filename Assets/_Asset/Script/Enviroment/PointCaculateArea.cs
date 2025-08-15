using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PointCaculateArea : MonoBehaviour
{
    [SerializeField] private GameObject _additionalArea;
    [SerializeField] private List<ReserveArea> _reserveArea = new List<ReserveArea>();
    private void OnEnable()
    {
        EventDispatcher<bool>.AddListener(Event.SpawnReserveArea.ToString(), SpawnAdditionalArea);
        EventDispatcher<Transform>.AddListener(Event.FromReserveToHole.ToString(), CheckArea);
    }
    private void OnDisable()
    {
        EventDispatcher<bool>.RemoveListener(Event.SpawnReserveArea.ToString(), SpawnAdditionalArea);
        EventDispatcher<Transform>.RemoveListener(Event.FromReserveToHole.ToString(), CheckArea);
    }

    public void SpawnReserve(int remainingChar, CharacterColor charColor)
    {
        for (int i = 0; i < _reserveArea.Count; i++)
        {
            if (_reserveArea[i].isActiveAndEnabled)
            {
                bool isValidArea = _reserveArea[i].IsValidArea();
                if (isValidArea)
                {
                    var UFOReserve = _reserveArea[i].UFO.GetComponent<UFOReserveArea>();
                    if (UFOReserve != null)
                    {
                        UFOReserve.charToReserveNum = remainingChar;
                        UFOReserve.charToReserveColor = charColor;
                        _reserveArea[i].ChangeAreaColor(charColor);
                    }    
                    _reserveArea[i].UFO.SetActive(true);
                    break;
                }
                else
                {
                    //No valid area
                    //Game over
                }
            }
        }
    }
    private void SpawnAdditionalArea(bool isSpawned)
    {

        _additionalArea.SetActive(isSpawned);
    }
    private void CheckArea(Transform target)
    {
        var holeCounter = target.GetComponent<PointHole>();
        Debug.Log("Target" + target.name);
        for (int i = 0; i < _reserveArea.Count; i++)
        {
            if (_reserveArea[i].areaColor == holeCounter.holeColor)
            {
                if (_reserveArea[i].reserveCharList.Count > holeCounter.counter)
                {
                    int numberToJump = _reserveArea[i].reserveCharList.Count - holeCounter.counter;
                    StartCoroutine(JumpSequence(target, _reserveArea[i].reserveCharList, numberToJump, _reserveArea[i]));
                }
                else
                {
                    StartCoroutine(JumpSequence(target, _reserveArea[i].reserveCharList, _reserveArea[i].reserveCharList.Count, _reserveArea[i]));
                }
            }    
        }    
    }
    private IEnumerator JumpSequence(Transform target, List<ReserveCharacter> reserveCharList, int numberToJump, ReserveArea reserveArea)
    {
        int counter = 0;
        for (int i = reserveCharList.Count - 1; i >= 0; i--)
        {
            
            if (counter < numberToJump)
            {
                Transform characterTransform = reserveCharList[i].transform;
                ReserveCharacter character = reserveCharList[i];
                float jumpHeight = 3f;
                int jumpNum = 1;
                float jumpDuration = 1f;
                character.transform.DOJump(target.position, jumpHeight, jumpNum, jumpDuration)
                    .SetEase(Ease.OutQuad);
                if (Vector3.Distance(character.transform.position, target.position) < 0.5f)
                {
                    character.DOKill();
                }    
                counter++;
                reserveCharList.RemoveAt(i);
                FormationManager.Instance.ReleasePoint(characterTransform);
            }  
            yield return new WaitForSeconds(0.2f);
            ChangeColorBack(reserveCharList, reserveArea);
        }
    }
    private void ChangeColorBack(List<ReserveCharacter> reserveCharList, ReserveArea reserveArea)
    {
        if (reserveCharList.Count <= 0)
        {
            reserveArea.areaSprite.color = new Color32(38, 81, 100, 255);
            reserveArea.areaColor = CharacterColor.None;
            Debug.Log("area change");
        }    
    }    
}
