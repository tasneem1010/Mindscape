using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class ButtonHighlightScript : MonoBehaviour, IPointerEnterHandler
{
    public AudioSource HoverAudio;


    public void OnPointerEnter(PointerEventData ped)
    {
        HoverAudio.Play();
    }

}