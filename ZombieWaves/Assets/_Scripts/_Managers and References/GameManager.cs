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
        Cursor.lockState = CursorLockMode.Confined;
        paused = false;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape) && !paused)
        {
            paused = true;
        }
        else if (Input.GetKeyDown(KeyCode.Escape) && paused)
        {
            paused = false;
        }
        
        if (paused)
        {
            Time.timeScale = 0;
            BackToPauseVoid();
        }
        else
        {
            Time.timeScale = 0;
            pauseP.SetActive(false);
        }
    }

    public void ControlsFPVoid()
    {
        pauseP.SetActive(false);
        controlsPP.SetActive(true);
        Cursor.lockState = CursorLockMode.Confined;
    }
    public void ControlsFSVoid()
    {
        startP.SetActive(false);
        controlsSP.SetActive(true);
        Cursor.lockState = CursorLockMode.Confined;
    }
    public void CreditsVoid()
    {
        creditsP.SetActive(true);
        pauseP.SetActive(false);
        Cursor.lockState = CursorLockMode.Confined;
    }
    public void StartVoid()
    {
        controlsSP.SetActive(false);
        startP.SetActive(true);
        pauseP.SetActive(false);
        Cursor.lockState = CursorLockMode.Confined;
    }
    public void StartGameVoid()
    {
        PlayerGO.SetActive(true);
        startP.SetActive(false);
        pauseP.SetActive(false);
        Cursor.lockState = CursorLockMode.Locked;
    } 

    public void BackToPauseVoid()
    {
        creditsP.SetActive(false);
        controlsPP.SetActive(false);
        startP.SetActive(false);
        pauseP.SetActive(true);
        Cursor.lockState = CursorLockMode.Confined;
    }

    public void QuitVoid()
    {
        Application.Quit();
    }
}
