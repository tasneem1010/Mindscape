using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenuTrigger : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        LoadingManager.Instance.LoadScene(GetComponentInParent<BackDoorScript>().SceneName);
    }

}
