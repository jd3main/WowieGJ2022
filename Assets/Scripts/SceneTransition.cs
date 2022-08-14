using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SceneTransition : MonoBehaviour
{
    public Image image;
    public float duration = 1;


    /*
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.A))
            Play();
    }
    */

    public void Play()
    {
        StartCoroutine(_Play());
    }

    private IEnumerator _Play()
    {
        Color c = image.color;
        c.a = 0;
        image.color = c;
        image.gameObject.SetActive(true);
        float t0 = Time.time;
        while (c.a < 1)
        {
            c.a = Mathf.Min((Time.time - t0)/duration, 1);
            image.color = c;
            yield return null;
        }
    }
}
