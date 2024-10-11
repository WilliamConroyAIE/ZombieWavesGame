using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Player : MonoBehaviour
{
    public int HP = 100;
    public GameObject bloodyScreen;
    public TextMeshProUGUI playerHealthUI;
    public GameObject gameOverUI;
    public GameObject standardCanvas;
    public bool isDead = false;
    public Camera playerCamera;

    public Button ToMainMenuButton;

    private void Start()
    {
        bloodyScreen.SetActive(false);
        standardCanvas.SetActive(true);
        playerCamera.GetComponent<Animator>().enabled = false;
        ToMainMenuButton.gameObject.SetActive(false);
    }

    private void Update()
    {
        playerHealthUI.text = $"Health: {HP}";
    }

    public void TakeDamage(int damageAmount)
    {
        HP -= damageAmount;

        if (HP <= 0)
        {
            print("Player Dead");
            Invoke("PlayerDead", 0f);
        }
        else
        {
            print("Player Hit");
            StartCoroutine(BloodyScreenEffect());
            playerHealthUI.text = $"Health: {HP}";
            SoundManager.Instance.playerChannel.PlayOneShot(SoundManager.Instance.playerHurtClip);
        }
    }

    private IEnumerator BloodyScreenEffect()
    {
        if (bloodyScreen.activeInHierarchy == false)
        {
            bloodyScreen.SetActive(true);
        }

        var image = bloodyScreen.GetComponentInChildren<Image>();
 
        Color startColor = image.color;
        startColor.a = 1f;
        image.color = startColor;
 
        float duration = 3f;
        float elapsedTime = 0f;
 
        while (elapsedTime < duration)
        {
            float alpha = Mathf.Lerp(1f, 0f, elapsedTime / duration);
 
            Color newColor = image.color;
            newColor.a = alpha;
            image.color = newColor;
 
            elapsedTime += Time.deltaTime;
 
            yield return null; ; 
        }


        if (bloodyScreen.activeInHierarchy)
        {
            bloodyScreen.SetActive(false);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("EnemyHand"))
        {
            if (!isDead)
            {
                TakeDamage(other.GetComponent<ZombieHand>().damage);
            }
        }
    }

    private void PlayerDead()
    {
        playerCamera.GetComponent<MouseLook>().enabled = false;
        GetComponent<PlayerMovement1>().enabled = false;

        playerCamera.GetComponent<Animator>().enabled = true;
        playerCamera.GetComponentInChildren<WeaponSway>().enabled = false;

        GetComponent<ScreenFader>().StartFade();

        StartCoroutine(ShowGameOverUI());
        standardCanvas.gameObject.SetActive(false);

        SoundManager.Instance.playerChannel.PlayOneShot(SoundManager.Instance.playerDeathClip);
    }

    private IEnumerator ShowGameOverUI()
    {
        yield return new WaitForSeconds(1f);
        gameOverUI.gameObject.SetActive(true);

        int waveSurvived = GlobalReference.Instance.waveNumber; 

        if (waveSurvived - 1 > SaveLoadManager.Instance.LoadHighScore())
        {
            SaveLoadManager.Instance.SaveHighScore(waveSurvived - 1);
        }

        yield return new WaitForSeconds(5f);
        ToMainMenuButton.gameObject.SetActive(true);
    }

    public void FromGameToMenu()
    {
        SceneManager.LoadScene(sceneName:"MainMenu");
    }
}
