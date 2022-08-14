using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Game : MonoBehaviour
{
    public SAN san;
    public Durability durability;

    [SerializeField]
    private LayerMask rayCastMask;
    private RaycastHit hit;
    private float rayCastDistance = 100f;

    public static Game instance;
    public OnGameStartEvent onGameStart;
    public OnGameEndEvent onGameEnd;

    private bool isInteracting = false;

    void Awake()
    {
        instance = this;
    }

    void Update()
    {
        if (!isInteracting && Input.GetMouseButtonDown(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out hit, rayCastDistance, rayCastMask))
            {
                Interactive obj = hit.transform.GetComponent<Interactive>();
                if (obj != null)
                {
                    isInteracting = true;
                    obj.Interact();
                }
            }
        }
    }


    public void StartGame()
    {
        
    }

    public void EndGame()
    {
        onGameEnd.Invoke();
    }

    public void DialogueClosed()
    {
        isInteracting = false;
    }

    public void ButtonActivate()
    {
        durability.DecreaseDurability(10);
    }
}


[System.Serializable]
public class OnGameStartEvent : UnityEvent { }

[System.Serializable]
public class OnGameEndEvent : UnityEvent { }
