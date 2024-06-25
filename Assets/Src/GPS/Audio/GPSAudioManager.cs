using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GPSAudioManager : MonoBehaviour
{
    public AudioClip clip;

    private AudioSource audioSource;
    // Start is called before the first frame update
    public void Play(GameObject gameObject, Int32 distance)
    {
        audioSource = gameObject.GetComponent<AudioSource>();
        if (audioSource == null) {
            audioSource = gameObject.AddComponent<AudioSource>();
        }

        audioSource.clip = clip;
        audioSource.loop = false;

        audioSource.Play();
    }

}
