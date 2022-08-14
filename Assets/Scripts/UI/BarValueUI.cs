using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BarValueUI : MonoBehaviour
{
    [SerializeField]
    private Transform barController;

    public void Render(float curValue, float maxValue)
    {
        if (curValue < 0)
            curValue = 0;
        if (curValue > maxValue)
            curValue = maxValue;
        
        barController.localScale = new Vector3(curValue / maxValue, 1, 1);
    }
}