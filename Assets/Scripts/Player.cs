using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public GameObject floatingParticlesPrefab;
    public AudioClip incorrectAudio;
    public AudioClip correctAudio;
    public AudioClip loseAudio;

    void Start()
    {
        if (Application.platform == RuntimePlatform.Android)
        {
            GameObject.Find("Move Joystick").SetActive(true);
            GameObject.Find("Look Joystick").SetActive(true);

        }
        Instantiate(floatingParticlesPrefab, transform.position, Quaternion.identity);
    }

    public void PlayIncorrect()
    {
        PlayAudio(incorrectAudio);
    }

    public void PlayLose()
    {
        PlayAudio(loseAudio);
    }
    public void PlayCorrect()
    {
        PlayAudio(correctAudio);
    }
    public void PlayAudio(AudioClip clip)
    {
        GetComponent<AudioSource>().clip = clip;
        GetComponent<AudioSource>().Play();
    }
}
