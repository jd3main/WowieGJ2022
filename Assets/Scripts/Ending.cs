using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class Ending : MonoBehaviour
{
    public DialogueSystem dialogueSystem;
    public List<RefText> endingInfos;

    private void Start()
    {
        dialogueSystem = DialogueSystem.instance;
        dialogueSystem.onDialogueClosed.AddListener(OnCloseDialogue);
    }

    public void OnCloseDialogue()
    {
        StartCoroutine(_OnCloseDialogue());
    }

    public IEnumerator _OnCloseDialogue()
    {
        yield return null;

        EndingManager endingManager = FindObjectOfType<EndingManager>();
        endingManager.dialoguePanel.SetActive(false);
        endingManager.endingDialogueAudioSource.clip = endingManager.endingDialogueAudioClip;
        dialogueSystem.textUI = endingManager.endingInfoTextUI;
        dialogueSystem.textUI.gameObject.SetActive(true);

        yield return null;

        string endingInfoStr = string.Join("\n", endingInfos.Select(refText=>refText.Value));
        dialogueSystem._Enqueue(endingInfoStr);

        dialogueSystem.onDialogueClosed.RemoveListener(OnCloseDialogue);
        dialogueSystem.onDialogueClosed.AddListener(endingManager.Restart);
    }

}
