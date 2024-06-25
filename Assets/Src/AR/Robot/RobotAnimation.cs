using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

[RequireComponent(typeof(RetrobotController), typeof(AudioSource))]
public class RobotAnimation : MonoBehaviour
{
    public ARManager arManager;
    public float timespan = 1.5f;  // seconds
    private float time;
    private bool started = false;
    private bool stopped = false;

    private RetrobotController botController;
    private AudioSource audioSource;
    
    void Awake() {
        botController = GetComponent<RetrobotController>();
        audioSource = GetComponent<AudioSource>();
    }

    void Update()
    {
        if (time <= Time.time && !started)
        {
            started = true;

            botController.DoAction("Loop_Talking_Head_Hands_Body");
            audioSource.Play();
        }
        if (time <= Time.time && !stopped && !audioSource.isPlaying) {
            stopped = true;

            botController.DoAction("Do_End");
            StartCoroutine(arManager.StopAR());
        }
    }

    void OnEnable()
    {
        time = Time.time + timespan;   

        started = false;
        stopped = false;
    }

    void OnDisable() 
    {
        started = false;
        stopped = false;
    }
}
