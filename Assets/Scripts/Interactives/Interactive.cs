using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public abstract class Interactive : MonoBehaviour
{
    public OnInteractEvent onInteract;

    public abstract void Interact();

    [System.Serializable]
    public class OnInteractEvent : UnityEvent { }
}
