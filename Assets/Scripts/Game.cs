using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

public class Game : MonoBehaviour
{
    [Header("Timer")]
    public TimerUI timerUI;
    public float maxTime = 120;
    [HideInInspector]
    public static float curTime;

    [Header("SAN")]
    public BarValueUI sanUI;
    public float maxSAN = 100;
    public float initSAN = 70;
    public float sanDropSpeed = 1f;
    [HideInInspector]
    public static float curSAN;

    [Header("Durability")]
    public BarValueUI durabilityUI;
    public float maxDurability = 100;
    public float initDurability = 100;
    [HideInInspector]
    public static float curDurability;

    [SerializeField]
    private LayerMask rayCastMask;
    private RaycastHit hit;
    private float rayCastDistance = 100f;

    public static Game instance;
    public OnGameStartEvent onGameStart;
    public OnGameEndEvent onGameEnd;

    private float oneSecondCounter;
    private bool isInteracting = false;
    Coroutine animationCoroutine;

    void Awake()
    {
        instance = this;

        oneSecondCounter = 0;
        curTime = 0;
        curSAN = initSAN;
        curDurability = initDurability;
    }

    void Update()
    {
        curTime += Time.deltaTime;
        if (curTime >= maxTime)
        {
            SceneManager.LoadScene("Ending");
        }
        oneSecondCounter += Time.deltaTime;
        if (oneSecondCounter >= 1)
        {
            oneSecondCounter -= 1;
            curSAN -= sanDropSpeed;
        }

        CheckMouseClick();
        UpdateUI();
    }

    private void CheckMouseClick()
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

    private void UpdateUI()
    {
        timerUI.Render(maxTime - curTime);
        sanUI.Render(curSAN, maxSAN);
        durabilityUI.Render(curDurability, maxDurability);
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
        curDurability += -6;
        curSAN += 5;
    }

    public void ButtonShieldActivate()
    {
        curDurability += -4;
        curSAN += 3;
    }

    public void ButtonAimActive()
    {
        curDurability += -3;
        curSAN += 3;
    }

    public void SmallButtonActive()
    {
        curDurability += Random.Range(-3f, 2f);
        curSAN += Random.Range(-3f, 3f);
    }

    public void SquareButtonActive()
    {
        curDurability += Random.Range(-3f, 0);
        curSAN += Random.Range(1f, 3f);
    }

    public void AccelatorActive()
    {
        curDurability += -8;
        curSAN += 4;
    }

    public void BreakActive()
    {
        curDurability += -10;
        curSAN += 4;
        animationCoroutine = StartCoroutine(ShakeCameraAnimation());
    }

    private IEnumerator ShakeCameraAnimation()
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
