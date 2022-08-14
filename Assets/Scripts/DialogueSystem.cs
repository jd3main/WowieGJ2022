using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using TMPro;
using Sirenix.OdinInspector;

public class DialogueSystem : MonoBehaviour
{
    [ShowInInspector]
    public static DialogueSystem instance;

    [Required]
    public AudioSource audioSource;
    [Required]
    public TextMeshPro textUI;
    public List<string> textQueue;
    public float playTextSpeed = 20;
    public string current;
    public KeyCode nextKey = KeyCode.Mouse0;
    public OnDialogueClosedEvent onDialogueClosed;

    bool nextKeyClicked = false;
    Coroutine animationCoroutine;


    private void Awake()
    {
        instance = this;
        current = null;
        if (audioSource == null)
            audioSource = GetComponent<AudioSource>();
    }

    void Start()
    {
    }

    private void Update()
    {
        if (Input.GetKeyDown(nextKey))
        {
            nextKeyClicked = true;
        }
        if (Input.GetKeyUp(nextKey))
        {
            nextKeyClicked = false;
        }
    }

    public static void Enqueue(string s)
    {
        instance._Enqueue(s);
    }

    public void _Enqueue(string s)
    {
        Debug.Log($"Enqueue({s})");
        textQueue.Add(s);
        if (current == null)
            PlayNext();
    }

    public void PlayNext()
    {
        if (textQueue.Count > 0)
        {
            textUI.text = textQueue[0];
            current = textQueue[0];
            textQueue.RemoveAt(0);
            animationCoroutine = StartCoroutine(RunDialogueAnimation());
        }
        else
            CloseDialogue();
    }

    public void CloseDialogue()
    {
        textUI.text = "";
        current = null;
        StopCoroutine(animationCoroutine);
        onDialogueClosed.Invoke();
    }

    public IEnumerator RunDialogueAnimation()
    {
        textUI.text = "";
        for (int i = 0; i < current.Length; i++)
        {
            textUI.text += current[i];
            if (GetNextKeyClicked())
            {
                yield return null;
                break;
            }
            if (current[i] != ' ')
                audioSource.Play();
            yield return new WaitForSeconds(1f/playTextSpeed);
            yield return null;
        }
        textUI.text = current + " ¡¿";
        while (true)
        {
            if (GetNextKeyClicked())
            {
                yield return null;
                break;
            }
            yield return null;
        }
        PlayNext();
    }

    private bool GetNextKeyClicked()
    {
        if (nextKeyClicked)
        {
            nextKeyClicked = false;
            return true;
        }
        return false;
    }

    [System.Serializable]
    public class OnDialogueClosedEvent : UnityEvent { }
}
