using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;
using Sirenix.OdinInspector;

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


    [Header("Interactions")]
    public float laserSAN = 5;
    public float laserDurability = -6;

    public float shieldSAN = 3;
    public float shieldDurability = -4;

    public float aimSAN = 4;
    public float aimDurability = -3;

    public float smallButtonSAN = -2;
    public float smallButtonDurability = 2;

    public float squareButtonSAN = 2;
    public float squareButtonDurbility = -2;

    public float acceleratorSAN = 4;
    public float acceleratorDurability = -8;

    public float breakSAN = 5;
    public float breakDurability = -10;

    [Header("Others")]
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
        curDurability += laserDurability;
        curSAN += laserSAN;
    }

    public void ButtonShieldActivate()
    {
        curSAN += shieldSAN;
        curDurability += shieldDurability;
    }

    public void ButtonAimActive()
    {
        curSAN += aimSAN;
        curDurability += aimDurability;
    }

    public void SmallButtonActive()
    {
        curSAN += smallButtonSAN;
        curDurability += smallButtonDurability;
    }

    public void SquareButtonActive()
    {
        curSAN += squareButtonSAN;
        curDurability += squareButtonDurbility;
    }

    public void AcceleratorActive()
    {
        curSAN += acceleratorSAN;
        curDurability += acceleratorDurability;
    }

    public void BreakActive()
    {
        curSAN += breakSAN;
        curDurability += breakDurability;
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
