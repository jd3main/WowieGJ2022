using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Meteoroid : MonoBehaviour
{
    public Animator anm;
    public float destroyDelay = 0.1f;
    public AudioSource SFX;
    private bool hit = false;

    private void Reset()
    {
        anm = GetComponent<Animator>();
        hit = false;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.gameObject.tag == "SpaceShip" && !hit)
        {
            hit = true;
            anm.SetTrigger("Collide");
            SFX.Play();
            StartCoroutine(MakeInvisible());
            Destroy(this.gameObject, 3f);
        }
        else if (collision.collider.gameObject.tag == "MeteoroidDestroyer")
        {
            Destroy(this.gameObject);
        }
    }

    private IEnumerator MakeInvisible()
    {
        yield return new WaitForSeconds(destroyDelay);
        this.gameObject.GetComponent<MeshRenderer>().enabled = false;
    }
}
