using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class DialogueSystem : MonoBehaviour
{
    public TextMeshPro textUI;
    public List<string> textQueue;
    public float playTextSpeed = 20;
    public string current;
    public KeyCode nextKey = KeyCode.Mouse0;
    
    bool nextKeyClicked = false;
    Coroutine animationCoroutine;

    void Start()
    {
        for (int i = 0; i < 100; i++)
        {
            string s = "";
            for (int j = 0; j < 10; j++)
            {
                s += i.ToString();
            }
            Enqueue(s);
        }
        PlayNext();
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


    public void Enqueue(string s)
    {
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
        StopCoroutine(animationCoroutine);
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
            yield return new WaitForSeconds(1f/playTextSpeed);
            yield return null;
        }
        textUI.text = current + " ��";
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
}