using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Interactive : MonoBehaviour
{
    public SAN san;
    public Durability durability;

    public abstract void StartInteraction();
    
    public void FinishInteraction()
    {
        
    }
    
}
