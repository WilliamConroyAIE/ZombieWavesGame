using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public bool paused;

    public GameObject PlayerGO;

    [Header("UI")]
    public GameObject controlsPP;
    public GameObject controlsSP, creditsP, pauseP, startP;

    public enum currentStates
    {
        onStart,
        onControls,
        onCredits,
        onPause,
        inGame
    }
    public currentStates cs;



    void Start()
    {
        startP.SetActive(true);
        creditsP.SetActive(false);
        controlsPP.SetActive(false);
        controlsSP.SetActive(false);
        pauseP.SetActive(false);        
    }

    void Update()
    {
        if (paused)
        {
            Time.timeScale = 0;
            pauseP.SetActive(true);
        }
        else
        {
            Time.timeScale = 0;
            pauseP.SetActive(false);
        }
    }

    void ControlsFPVoid()
    {
        pauseP.SetActive(false);
        controlsPP.SetActive(true);
    }
    void ControlsFSVoid()
    {
        startP.SetActive(false);
        controlsSP.SetActive(true);
    }
    void CreditsVoid()
    {
        creditsP.SetActive(true);
        pauseP.SetActive(false);
    }
    void StartVoid()
    {
        controlsSP.SetActive(false);
        startP.SetActive(true);
    }
    void StartGameVoid()
    {
        PlayerGO.SetActive(true);
        startP.SetActive(false);
    } 
}
