using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class Timer : MonoBehaviour
{
    public float maxTime = 120f;
    public TextMeshPro timerUI;

    [HideInInspector]
    public float RemainingTime = 0;
    
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

        string DisplayTime = RemainingTime.ToString("F2");
        timerUI.text = string.Format("Target Distance: " + DisplayTime + " ly");
    }
}

