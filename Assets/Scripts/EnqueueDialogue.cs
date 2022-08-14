using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnqueueDialogue : MonoBehaviour
{
    public List<RefText> dialogues = new List<RefText>();

    private void OnEnable()
    {
        foreach (RefText t in dialogues)
        {
            DialogueSystem.Enqueue(t);
        }
    }
}
