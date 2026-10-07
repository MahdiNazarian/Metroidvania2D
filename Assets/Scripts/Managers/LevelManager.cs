using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
public class LevelManager : MonoBehaviour
{
    public static LevelManager instance;
    [SerializeField] private float fadeDuration;
    private CanvasGroup canvasGroup;
    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        canvasGroup = GetComponent<CanvasGroup>();
        StartCoroutine(FadeToTransparent());
    }

    public void ResetLevel()
    {
        StartCoroutine(FadeToBlackInt(SceneManager.GetActiveScene().buildIndex));
    }

    public void LoadLevelString(string levelToLoad)
    {
        StartCoroutine(FadeToBlackString(levelToLoad));
    }

    public IEnumerator FadeToTransparent()
    {
        float fadeStartAlpha = canvasGroup.alpha;
        float fadeToAlpha = 0;
        float time = 0;
        while(time < fadeDuration)
        {
            time += Time.deltaTime;
            canvasGroup.alpha = Mathf.Lerp(fadeStartAlpha, fadeToAlpha, time / fadeDuration);
            yield return null;
        }
        canvasGroup.alpha = 0;
    }
    public IEnumerator FadeToBlackInt(int buildIndex)
    {
        float fadeStartAlpha = canvasGroup.alpha;
        float fadeToAlpha = 1;
        float time = 0;
        while (time < fadeDuration)
        {
            time += Time.deltaTime;
            canvasGroup.alpha = Mathf.Lerp(fadeStartAlpha, fadeToAlpha, time / fadeDuration);
            yield return null;
        }
        canvasGroup.alpha = 1;
        SceneManager.LoadScene(buildIndex);
    }
    public IEnumerator FadeToBlackString(string sceneName)
    {
        float fadeStartAlpha = canvasGroup.alpha;
        float fadeToAlpha = 1;
        float time = 0;
        while (time < fadeDuration)
        {
            time += Time.deltaTime;
            canvasGroup.alpha = Mathf.Lerp(fadeStartAlpha, fadeToAlpha, time / fadeDuration);
            yield return null;
        }
        canvasGroup.alpha = 1;
        SceneManager.LoadScene(sceneName);
    }
}
