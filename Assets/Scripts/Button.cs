using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Button : Interactive
{
    public List<RefText> dialogues = new List<RefText>();
    [SerializeField]
    private float recoverTime = 1f;

    private float pressedTime;
    private bool isPressed;
    private Vector3 originalScale;
    

    private void Start()
    {
        isPressed = false;
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
            }
        }
    }

    public override void Interact()
    {
        onInteract.Invoke();
        
        transform.localScale = new Vector3(originalScale.x, originalScale.y * 0.5f, originalScale.z);
        isPressed = true;
        pressedTime = 0;

        foreach (RefText t in dialogues)
        {
            DialogueSystem.Enqueue(t);
        }
    }
}
