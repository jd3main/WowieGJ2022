using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class Timer : MonoBehaviour
{
    public float maxTime = 120f;
    public TextMeshPro timerUI;

    private float RemainingTime = 0;
    
    void Start()
    {
        RemainingTime = maxTime;
    }

    void Update()
    {
        RemainingTime -= Time.deltaTime;

        if (RemainingTime < 0)
        {
            SceneManager.LoadScene("Ending");
        }

        int intTime = Mathf.RoundToInt(RemainingTime);
        timerUI.text = string.Format("{0:000}", intTime);
    }
}

