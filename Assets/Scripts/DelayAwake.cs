using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DelayAwake : MonoBehaviour
{
    public Behaviour behaviour;
    public float delay = 0.1f;

    void Start()
    {
        StartCoroutine(DelayEnable());
    }

    private IEnumerator DelayEnable()
    {
        yield return new WaitForSeconds(delay);
        behaviour.enabled = true;
    }
}
