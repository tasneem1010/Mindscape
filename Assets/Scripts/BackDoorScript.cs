using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackDoorScript : MonoBehaviour
{

    public string Text;
    public string SceneName;

    private void Start()
    {
        GetComponentInChildren<TMPro.TextMeshPro>().text = Text;
    }

}
