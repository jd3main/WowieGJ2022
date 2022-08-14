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
    public float remainingTime = 0;
    
    void Start()
    {
        remainingTime = maxTime;
    }

    void Update()
    {
        remainingTime -= Time.deltaTime;

        if (remainingTime < 0)
        {
            SceneManager.LoadScene("Ending");
        }

        string displayTime = (remainingTime / 10f).ToString("F2");
        timerUI.text = string.Format("Target Distance: " + displayTime + " ly");
    }
}

