using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Durability : MonoBehaviour
{
    public float maxDurability = 100;
    public float initDurability = 70;

    private float curDurability = 70;
    private Transform barController;

    void Start()
    {
        curDurability = initDurability;
        barController = transform.Find("Bar Controller");
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            IncreaseDurability(10);
        }
        else if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            DecreaseDurability(10);
        }
        UpdateUI();
    }

    public void DecreaseDurability(float value)
    {
        if (curDurability - value < 0)
        {
            curDurability = 0;
        }
        else
        {
            curDurability -= value;
        }
    }

    public void IncreaseDurability(float value)
    {
        curDurability = Mathf.Min(curDurability + value, maxDurability);
    }

    private void UpdateUI()
    {
        barController.localScale = new Vector3(curDurability / maxDurability, 1, 1);
    }
}