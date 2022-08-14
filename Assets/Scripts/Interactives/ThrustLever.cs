using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThrustLever : Interactive
{
    public List<RefText> dialogues = new List<RefText>();
    public Transform handle;
    public float moveDistance = 0.25f;
    public float moveDuration = 1f;

    [SerializeField]
    private bool isUp = true;
    [SerializeField]
    private bool autoReverse = false;

    private bool isMoving = false;
    private bool reversed = false;
    private float zPosition = 0;
    
    private void Start()
    {
        if (!isUp)
        {
            zPosition = -moveDistance;
            handle.transform.localPosition = new Vector3(0, 0, zPosition);
        }
        if (autoReverse)
            moveDuration /= 2;
    }

    private void Update()
    {
        if (isMoving)
        {
            float delta = moveDistance * (Time.deltaTime / moveDuration);
            if (isUp)
            {
                zPosition -= delta;
                if(zPosition < -moveDistance)
                {
                    zPosition = -moveDistance;
                    if (!autoReverse || reversed)
                        isMoving = false;
                    isUp = false;
                    reversed = true;
                }
            }
            else
            {
                zPosition += delta;
                if(zPosition > 0)
                {
                    zPosition = 0;
                    if (!autoReverse || reversed)
                        isMoving = false;
                    isUp = true;
                    reversed = true;
                }
            }

            handle.transform.localPosition = new Vector3(0, 0, zPosition);
        }
    }

    public override void Interact()
    {
        onInteract.Invoke();

        isMoving = true;
        reversed = false;
        if (autoReverse)
        {
            foreach (RefText t in dialogues)
            {
                DialogueSystem.Enqueue(t);
            }
        }
        else
        {
            if (isUp)
                DialogueSystem.Enqueue(dialogues[0]);
            else
                DialogueSystem.Enqueue(dialogues[1]);
        }
    }
}
