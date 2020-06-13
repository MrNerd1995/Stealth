using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneFadeInOut : MonoBehaviour
{
    public float fadeSpeed = 1.5f;

    private bool sceneStarting = true;

    CanvasGroup canvasGroup;

    private void Awake()
    {
        canvasGroup = GetComponent<CanvasGroup>();
    }

    private void Update()
    {
        if (sceneStarting)
        {
            StartCoroutine(StartScene());
        }    
    }

    private IEnumerator StartScene()
    {
        yield return FadeToClear();
        yield return sceneStarting = false;
    }

    public IEnumerator EndScene()
    {
        yield return FadeToBlack();
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        yield return null;
    }

    public IEnumerator FadeToBlack()
    {
        while (canvasGroup.alpha < 1)
        {
            canvasGroup.alpha += Time.deltaTime / fadeSpeed;
            yield return null;
        }
    }

    public IEnumerator FadeToClear()
    {
        while (canvasGroup.alpha > 0)
        {
            canvasGroup.alpha -= Time.deltaTime / fadeSpeed;
            yield return null;
        }
    }
}
