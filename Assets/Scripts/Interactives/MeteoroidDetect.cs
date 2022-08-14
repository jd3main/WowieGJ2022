using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeteoroidDetect : Interactive
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Meteoroid")
        {
            onInteract.Invoke();
        }
    }

    public override void Interact()
    {
        
    }
}
