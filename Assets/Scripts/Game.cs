using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;
using Sirenix.OdinInspector;

public class Game : MonoBehaviour
{
    public static RelationStatus relationStatus;
    public static ShipStatus shipStatus;

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

    public float meteoroidHitSAN = 3;
    public float meteoroidHitDurability = -3;

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
            EndGame();
        }
        oneSecondCounter += Time.deltaTime;
        if (oneSecondCounter >= 1)
        {
            oneSecondCounter -= 1;
            curSAN -= sanDropSpeed;
        }

        CheckMouseClick();
        UpdateUI();
        if (CheckGameOver())
        {
            EndGame();
        }
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

    public bool CheckGameOver()
    {
        if (sanLevel == SanLevel.High)
            relationStatus = RelationStatus.Seeing;
        if (sanLevel == SanLevel.Normal)
            relationStatus = RelationStatus.Friend;
        if (sanLevel == SanLevel.Low)
            relationStatus = RelationStatus.Normal;
        if (sanLevel == SanLevel.VeryLow)
            relationStatus = RelationStatus.Crazy;

        if (durabilityLevel == DurabilityLevel.Distroyed)
        {
            shipStatus = ShipStatus.Boom;
            return true;
        }

        if (durabilityLevel == DurabilityLevel.Danger && travelProgress == TravelProgress.Near)
        {
            shipStatus = ShipStatus.Drift;
            return true;
        }

        if (durabilityLevel == DurabilityLevel.Danger && travelProgress == TravelProgress.Almost)
        {
            shipStatus = ShipStatus.Crash;
            return true;
        }

        if (travelProgress == TravelProgress.Arrived)
        {
            shipStatus = ShipStatus.Arrive;
            return true;
        }

        return false;
    }

    public SanLevel sanLevel
    {
        get
        {
            if (curSAN <= 30)
                return SanLevel.VeryLow;
            else if (curSAN <= 60)
                return SanLevel.Low;
            else if (curSAN <= 80)
                return SanLevel.Normal;
            else
                return SanLevel.High;
        }
    }

    public DurabilityLevel durabilityLevel
    {
        get
        {
            if (curDurability <= 0)
                return DurabilityLevel.Distroyed;
            else if (curDurability <= 30)
                return DurabilityLevel.Danger;
            else if (curDurability <= 50)
                return DurabilityLevel.Poor;
            else
                return DurabilityLevel.Okay;
        }
    }

    public TravelProgress travelProgress
    {
        get
        {
            float leftTime = maxTime - curTime;
            if (leftTime <= 0)
                return TravelProgress.Arrived;
            else if (leftTime <= 30)
                return TravelProgress.Almost;
            else if (leftTime <= 50)
                return TravelProgress.Near;
            else
                return TravelProgress.Far;
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

    public void MeteoroidHit()
    {
        curSAN += meteoroidHitSAN;
        curDurability += meteoroidHitDurability;
        isInteracting = true;
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


public enum SanLevel
{
    VeryLow,
    Low,
    Normal,
    High,
}

public enum DurabilityLevel
{
    Distroyed,
    Danger,
    Poor,
    Okay,
}

public enum TravelProgress
{
    Far,
    Near,
    Almost,
    Arrived,
}


[System.Serializable]
public class OnGameStartEvent : UnityEvent { }

[System.Serializable]
public class OnGameEndEvent : UnityEvent { }
