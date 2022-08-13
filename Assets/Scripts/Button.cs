using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Button : Interactive
{
    [SerializeField]
    private float recoverTime = 1f;
    
    private float pressedTime;
    private bool isPressed;
    private Vector3 originalScale;
    

    private void Start()
    {
        isPressed = false;
        pressedTime = 0;
        originalScale = transform.localScale;
    }

    private void Update()
    {
        if (isPressed)
        {
            pressedTime += Time.deltaTime;
            if (pressedTime > recoverTime)
            {
                transform.localScale = originalScale;
                isPressed = false;
                pressedTime = 0;
            }
        }
    }

    public override void StartInteraction()
    {   
        transform.localScale = new Vector3(originalScale.x, originalScale.y * 0.5f, originalScale.z);
        isPressed = true;
        pressedTime = 0;
        durability.DecreaseDurability(10);
    }
}
