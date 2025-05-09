using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LoadingManager : MonoBehaviour
{
    public static LoadingManager Instance; 
    private string sceneToLoad;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
    }

    public void LoadScene(string targetScene)
    {
        sceneToLoad = targetScene;
        SceneManager.LoadScene("LoadingScene");
    }

    public IEnumerator LoadSceneAsync(Image progressBar)
    {
        // Start asynchronous operation
        AsyncOperation operation = SceneManager.LoadSceneAsync(sceneToLoad);
        operation.allowSceneActivation = false;

        float fakeProgress = 0f; // To simulate progress visually
        float delayTime = 2f;    // Simulated delay (in seconds)
        float startTime = Time.time;

        while (!operation.isDone)
        {
            // Simulated progress during the delay
            if (Time.time - startTime < delayTime)
            {
                // Increment fake progress over time
                fakeProgress += Time.deltaTime / delayTime;
                fakeProgress = Mathf.Clamp01(fakeProgress);
                if (progressBar != null)
                {
                    progressBar.fillAmount = fakeProgress;
                }
            }
            else
            {
                // Switch to actual operation progress after the delay
                float progress = Mathf.Clamp01(operation.progress / 0.9f);
                if (progressBar != null)
                {
                    progressBar.fillAmount = progress;
                }
            }

            // Activate the scene once fully loaded and delay is complete
            if (operation.progress >= 0.9f && Time.time - startTime >= delayTime)
            {
                if (progressBar != null) progressBar.fillAmount = 1f;
                operation.allowSceneActivation = true;
            }

            yield return null; // Wait for the next frame
        }
    }

}
