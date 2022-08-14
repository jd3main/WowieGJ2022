using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Game : MonoBehaviour
{
    public SAN san;
    public Durability durability;
    public Timer timer;

    public float sanDecreaseSpeed = 1f;

    [SerializeField]
    private LayerMask rayCastMask;
    private RaycastHit hit;
    private float rayCastDistance = 100f;

    public static Game instance;
    public OnGameStartEvent onGameStart;
    public OnGameEndEvent onGameEnd;

    private bool isInteracting = false;
    private int curTime;
    Coroutine animationCoroutine;

    void Awake()
    {
        instance = this;
        curTime = Mathf.RoundToInt(timer.maxTime);
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

        if (Mathf.RoundToInt(timer.RemainingTime) < curTime)
        {
            curTime--;
            san.DecreaseSAN(sanDecreaseSpeed);
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

    public void ButtonLaserActivate()
    {
        durability.DecreaseDurability(6);
        san.IncreaseSAN(5);
    }

    public void ButtonShieldActivate()
    {
        durability.DecreaseDurability(4);
        san.IncreaseSAN(3);
    }

    public void ButtonAimActive()
    {
        durability.DecreaseDurability(3);
        san.IncreaseSAN(3);
    }

    public void SmallButtonActive()
    {
        durability.IncreaseDurability(Random.Range(-3f, 2f));
        san.DecreaseSAN(Random.Range(-3f, 3f));
    }

    public void SquareButtonActive()
    {
        durability.DecreaseDurability(Random.Range(0, 3));
        san.IncreaseSAN(Random.Range(1, 3));
    }

    public void AccelatorActive()
    {
        durability.DecreaseDurability(8);
        san.IncreaseSAN(4);
    }

    public void BreakActive()
    {
        durability.DecreaseDurability(10);
        san.IncreaseSAN(4);
        animationCoroutine = StartCoroutine(ShakeCameraAnimation());
    }

    public IEnumerator ShakeCameraAnimation()
    {
        float shakeDuration = 1f;
        float shakeAmp = 0.005f;
        float endTime = Time.time + shakeDuration;
        Camera camera = Camera.main;
        Vector3 cameraPosition = camera.transform.position;

        while(Time.time < endTime)
        {
            camera.transform.position = cameraPosition + new Vector3(Random.Range(-shakeAmp, shakeAmp), Random.Range(-shakeAmp, shakeAmp), Random.Range(-shakeAmp, shakeAmp));
            yield return null;
        }
        camera.transform.position = cameraPosition;
    }
}


[System.Serializable]
public class OnGameStartEvent : UnityEvent { }

[System.Serializable]
public class OnGameEndEvent : UnityEvent { }
