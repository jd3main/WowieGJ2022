using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Meteoroid : MonoBehaviour
{
    public Animator anm;
    public float destroyDelay = 0.1f;

    private void Reset()
    {
        anm = GetComponent<Animator>();
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.gameObject.tag == "SpaceShip")
        {
            anm.SetTrigger("Collide");
            Destroy(this.gameObject, destroyDelay);
        }
    }
}
