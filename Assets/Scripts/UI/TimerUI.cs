using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class TimerUI : MonoBehaviour
{
    public TextMeshPro timerText;
    
    public void Render(float value)
    {
        string distance = (value / 10f).ToString("F2");
        timerText.text = string.Format("Target Distance: " + distance + " ly");
    }
}

