using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Button : Interactive
{
    public List<RefText> dialogues = new List<RefText>();
    [SerializeField]
    private float recoverTime = 1f;
    [SerializeField]
    private bool randomDialogue = false;

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

        if (randomDialogue)
        {
            int repeat = Random.Range(1, 3);
            for (int i=0; i<repeat; i++)
            {
                DialogueSystem.Enqueue(dialogues[Random.Range(0, dialogues.Count)]);
            }
        }
        else
        {
            foreach (RefText t in dialogues)
            {
                DialogueSystem.Enqueue(t);
            }
        }
    }
}
