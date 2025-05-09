using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuManager : MonoBehaviour
{

    public void StartGame()
    {
        StartCoroutine(LoadScene("MainScene"));
    }
    public IEnumerator LoadScene(string scene)
    {
        yield return new WaitForSeconds(0.2f);
        LoadingManager.Instance.LoadScene(scene);
    }


    public void CloseGame()
    {
        Application.Quit();
    }
}
