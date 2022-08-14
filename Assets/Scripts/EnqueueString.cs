using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnqueueString : MonoBehaviour
{
    [TextArea]
    public string s;
    void Start()
    {
        DialogueSystem.Enqueue(s);
    }
}
