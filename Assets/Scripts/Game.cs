using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Game : MonoBehaviour
{
    public static Game instance;
    public float progress;
    public OnGameStartEvent onGameStart;
    public OnGameEndEvent onGameEnd;

    

    void Awake()
    {
        instance = this;
        progress = 0;
    }

    void Update()
    {
        if (progress >= 1)
        {
            EndGame();
        }
    }


    public void StartGame()
    {
        
    }


    public void EndGame()
    {
        onGameEnd.Invoke();
    }
}


[System.Serializable]
public class OnGameStartEvent : UnityEvent { }

[System.Serializable]
public class OnGameEndEvent : UnityEvent { }
