using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class SceneTransition : MonoBehaviour
{
    public Image image;
    public float outDuration = 1;
    public float inDuration = 1;
    public string sceneName;
    public AudioSource audioToFadeOut;

    private float initialVolumn;

    void Awake()
    {
        DontDestroyOnLoad(this.gameObject);
    }

    /*
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.A))
            Play();
    }
    */

    public void Play(string sceneName = null)
    {
        if (!string.IsNullOrEmpty(sceneName))
            this.sceneName = sceneName;
        StartCoroutine(_Play());
    }

    private IEnumerator _Play()
    {
        Color c = image.color;
        c.a = 0;
        image.color = c;
        image.gameObject.SetActive(true);
        if (audioToFadeOut != null)
            initialVolumn = audioToFadeOut.volume;
        float t0 = Time.time;
        while (c.a < 1)
        {
            float value = Mathf.Min((Time.time - t0) / inDuration, 1);
            c.a = value;
            if (audioToFadeOut != null)
                audioToFadeOut.volume = initialVolumn * (1-value);
            image.color = c;
            yield return null;
        }

        SceneManager.LoadScene(sceneName);

        t0 = Time.time;
        while (c.a > 0)
        {
            float value = Mathf.Max(1 - (Time.time - t0) / outDuration, 0);
            c.a = value;
            if (audioToFadeOut != null)
                audioToFadeOut.volume = initialVolumn * (1 - value);
            image.color = c;
            yield return null;
        }

        Destroy(this.gameObject);
    }
}
