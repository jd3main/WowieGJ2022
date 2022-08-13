using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SAN : MonoBehaviour
{
    public float maxSAN = 100;
    public float initSAN = 70;

    private float curSAN = 70;
    private Transform barController;

    void Start()
    {
        curSAN = initSAN;
        barController = transform.Find("Bar Controller");
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            IncreaseSAN(10);
        }
        else if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            DecreaseSAN(10);
        }
        UpdateUI();
    }

    public void DecreaseSAN(float value)
    {
        if (curSAN - value < 0)
        {
            curSAN = 0;
        }
        else
        {
            curSAN -= value;
        }
    }

    public void IncreaseSAN(float value)
    {
        curSAN = Mathf.Min(curSAN + value, maxSAN);
    }

    private void UpdateUI()
    {
        barController.localScale = new Vector3(curSAN / maxSAN, 1, 1);
    }
}