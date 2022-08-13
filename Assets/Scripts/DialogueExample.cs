using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueExample : MonoBehaviour
{
    public List<RefText> dialogues = new List<RefText>();

    private void Start()
    {
        foreach (RefText t in dialogues)
        {
            DialogueSystem.Enqueue(t);
        }
    }
}
