using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public TMP_Text highScoreUI;

    [Header("Audio")]
    public AudioClip backGroundMusic;
    //public AudioClip clickSound;
    public AudioSource musicChannel/*, clickChannel*/;

    [Header("ControlPanel")]
    public Button startButton; 
    public Button controlsButton, exitButton, altNextButton, altBackButton;
    public GameObject controlsPanel, Body1, Body2, Body3;

    internal bool isOnPage1, isOnPage2, isOnPage3, isOnControlPanel, wishesForNewPage;

    void Start()
    {
        musicChannel.PlayOneShot(backGroundMusic);
        
        int highScore = SaveLoadManager.Instance.LoadHighScore();
        highScoreUI.text = $"Top Wave Survived: {highScore}";

        startButton.gameObject.SetActive(true);
        controlsButton.gameObject.SetActive(true);
        exitButton.gameObject.SetActive(true);
        altNextButton.gameObject.SetActive(false);
        altBackButton.gameObject.SetActive(false);

        controlsPanel.SetActive(false);
        Body1.SetActive(false);
        Body2.SetActive(false);
        Body3.SetActive(false);
        isOnControlPanel = false;
    }

    public void StartNewGame()
    {
        musicChannel.Stop();
        
        startButton.gameObject.SetActive(false);
        controlsButton.gameObject.SetActive(false);
        exitButton.gameObject.SetActive(false);
        altNextButton.gameObject.SetActive(false);
        altBackButton.gameObject.SetActive(false);

        controlsPanel.SetActive(false);
        Body1.SetActive(false);
        Body2.SetActive(false);
        Body3.SetActive(false);
        wishesForNewPage = false;

        SceneManager.LoadScene(sceneName:"MainScene");
    }

    public void ExitApplication()
    {
        #if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
        #else
            //Application.Quit();
        #endif
    }

    public void OpenControlPanel()
    {
        highScoreUI.text = "";
        
        startButton.gameObject.SetActive(false);
        controlsButton.gameObject.SetActive(false);
        exitButton.gameObject.SetActive(false);
        altNextButton.gameObject.SetActive(true);
        altBackButton.gameObject.SetActive(true);

        controlsPanel.SetActive(true);

        isOnControlPanel = true;
        isOnPage1 = true;
    }

    public void BackToMenu()
    {
        int highScore = SaveLoadManager.Instance.LoadHighScore();
        highScoreUI.text = $"Top Wave Survived: {highScore}";

        startButton.gameObject.SetActive(true);
        controlsButton.gameObject.SetActive(true);
        exitButton.gameObject.SetActive(true);
        altNextButton.gameObject.SetActive(false);
        altBackButton.gameObject.SetActive(false);

        Body1.SetActive(false);
        Body2.SetActive(false);
        Body3.SetActive(false);

        controlsPanel.SetActive(false);
        wishesForNewPage = false;
    }

    private void Update()
    {
        if (isOnControlPanel)
        {
            if (isOnPage1 && wishesForNewPage)
            {
                isOnPage1 = false;
                isOnPage2 = true;
                wishesForNewPage = false;
            }
            if (isOnPage2 && wishesForNewPage)
            {
                isOnPage2 = false;
                isOnPage3 = true;
                wishesForNewPage = false;
            }
            if (isOnPage3 && wishesForNewPage)
            {
                isOnPage3 = false;
                isOnPage1 = true;
                wishesForNewPage = false;
            }

            if (isOnPage1)
            {
                Body1.SetActive(true);
                Body2.SetActive(false);
                Body3.SetActive(false);
            }
            
            if (isOnPage2)
            {
                Body1.SetActive(false);
                Body2.SetActive(true);
                Body3.SetActive(false);
            }

            if (isOnPage3)
            {
                Body1.SetActive(false);
                Body2.SetActive(false);
                Body3.SetActive(true);
            }
        }

        if (musicChannel.isPlaying == false)
      {
         musicChannel.PlayOneShot(backGroundMusic);
      }
    }

    public void NewPageWisher()
    {
        wishesForNewPage = true;
    }
}
