using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class TargetScript : MonoBehaviour
{
    public GameObject grid;
    public GameObject subtitle;
    public GameObject title;

    public void OnTargetHit()
    {
        subtitle.gameObject.SetActive(false);
        grid.gameObject.SetActive(true);
        gameObject.SetActive(false);
        title.gameObject.SetActive(false);

    }
}
