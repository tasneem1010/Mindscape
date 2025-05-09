using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MemoryGameTrigger : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        LoadingManager.Instance.LoadScene(GetComponentInParent<MemoryGameDoor>().SceneName);
    }
}
