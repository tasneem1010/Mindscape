using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MemoryGameDoor : MonoBehaviour
{
    public string GameName;
    public string SceneName;

    private void Start()
    {
        GetComponentInChildren<TMPro.TextMeshPro>().text = GameName;
    }
}
