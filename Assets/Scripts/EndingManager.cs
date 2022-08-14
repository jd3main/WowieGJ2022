using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.Linq;
using TMPro;

public class EndingManager : MonoBehaviour
{
    public RelationStatus relationShip;
    public ShipStatus spaceShip;
    public TextMeshPro endingInfoTextUI;
    public GameObject dialoguePanel;
    public SceneTransition transition;

    private void Start()
    {
        string name = $"Ending_{relationShip.ToString()[0]}_{spaceShip.ToString()[0]}";
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
        transition.duration = 2.0f;
        transition.Play();
        yield return new WaitForSeconds(2.0f);
        SceneManager.LoadScene("SpaceShip");
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
