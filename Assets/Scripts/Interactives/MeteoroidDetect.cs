using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeteoroidDetect : Interactive
{
    public List<RefText> dialogues = new List<RefText>();

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Meteoroid")
        {
            Interact();
        }
    }

    public override void Interact()
    {
        onInteract.Invoke();
        DialogueSystem.Enqueue(dialogues[Random.Range(0, dialogues.Count)]);
    }
}
