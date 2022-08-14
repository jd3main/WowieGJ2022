using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.Linq;
using TMPro;
using Sirenix.OdinInspector;

public class EndingManager : MonoBehaviour
{
    public RelationStatus relationShip;
    public ShipStatus shipStatus;
    public TextMeshPro endingInfoTextUI;
    [Required]
    public AudioSource endingDialogueAudioSource;
    public AudioClip endingDialogueAudioClip = null;
    public GameObject dialoguePanel;
    public SceneTransition transition;

    private void Start()
    {
        relationShip = Game.relationStatus;
        shipStatus = Game.shipStatus;

        string name = $"Ending_{relationShip.ToString()[0]}_{shipStatus.ToString()[0]}";
        var endings = FindObjectsOfType<Ending>(true);
        var targetEnding = endings.Where(e => e.gameObject.name == name).First();
        GameObject endingGO = targetEnding.gameObject;
        endingGO.SetActive(true);
    }


    public void Restart()
    {
        StartCoroutine(_Restart());
    }

    public IEnumerator _Restart()
    {
        transition.Play();
        yield return new WaitForSeconds(2.0f);
    }
}

public enum RelationStatus
{
    Normal,
    Crazy,
    Friend,
    Seeing,
}

public enum ShipStatus
{
    Arrive,
    Crash,
    Drift,
    Boom,
}
