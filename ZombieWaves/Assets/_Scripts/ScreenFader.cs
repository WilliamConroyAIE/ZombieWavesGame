using UnityEngine;
using UnityEngine.UI;
using System.Collections;
 
public class ScreenFader : MonoBehaviour
{
    public Image fadeImage;
    public float fadeDuration = 7.0f;
    public bool isToBlackout = false;
 
    public void Start()
    {
        fadeImage.enabled = false;
        fadeImage.gameObject.SetActive(false);
    }

    public void Update()
    {
        if (isToBlackout)
            StartFade();
    }

    public void StartFade()
    {
        StartCoroutine(FadeOut());
        fadeImage.gameObject.SetActive(true);
    }
 
    private IEnumerator FadeOut()
    {
        float timer = 0f;
        Color startColor = fadeImage.color;
        Color endColor = new Color(0f, 0f, 0f, 1f); // Black with alpha 1.
 
        while (timer < fadeDuration)
        {
            // Interpolate the color between start and end over time.
            fadeImage.color = Color.Lerp(startColor, endColor, timer / fadeDuration);
            timer += Time.deltaTime;
            yield return null;
        }
 
        // Ensure the image is completely black at the end.
        fadeImage.color = endColor;

        yield return new WaitForSeconds(1f);

        SoundManager.Instance.playerChannel.PlayOneShot(SoundManager.Instance.gameOverClip);
    }
}