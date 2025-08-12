using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : Singleton<GameManager>
{
    public int charToPoolCount;
    public bool isCounting;
    void Start()
    {
        InitGame();
    }
    private void InitGame()
    {
        charToPoolCount = 0;
        isCounting = false;
    }
    public void CountCharToPool()
    {
        charToPoolCount++;
        isCounting = true;
    }    
}
