
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LoadingScreenController : MonoBehaviour
{
    public Image ProgressBar;
    public TextMeshProUGUI LoadingText;        

    void Start()
    {
        StartCoroutine(AnimateLoadingText());

        if (LoadingManager.Instance != null)
        {
            StartCoroutine(LoadingManager.Instance.LoadSceneAsync(ProgressBar));
        }
    }
    IEnumerator AnimateLoadingText()
    {
        int dotCount = 0;

        while (true) 
        {
            LoadingText.text = "Loading." + new string('.', dotCount);
            dotCount = (dotCount + 1) % 4;

            yield return new WaitForSeconds(0.5f); 
        }
    }
}

