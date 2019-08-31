using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VacuumSound : MonoBehaviour
{
    PlayerHealth player;
    AudioSource audio;
    [SerializeField]
    AudioClip start;
    [SerializeField]
    AudioClip middle;
    [SerializeField]
    AudioClip end;
    bool hasStarted = false;
    // Start is called before the first frame update
    void Start()
    {
        player = FindObjectOfType<PlayerHealth>();
        audio = GetComponentInChildren<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        if (player.isVacuuming)
        {
            if (!hasStarted)
            {
                audio.loop = false;
                audio.clip = start;
                audio.Play();
                hasStarted = true;
            }
            if (audio.clip == start && !audio.isPlaying)
            {
                audio.clip = middle;
                audio.loop = true;
                audio.Play();
            }
        }
        if (!player.isVacuuming && hasStarted)
        {
            audio.loop = false;
            audio.clip = end;
            audio.Play();
            hasStarted = false;
        }
    }
}
