using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MakeSound : MonoBehaviour
{
    private AudioSource source;
    // Start is called before the first frame update
    void Start()
    {
        if (gameObject.GetComponentInChildren<AudioSource>()) // Check for audio source
        {
            source = gameObject.GetComponentInChildren<AudioSource>();
        }
        else Debug.LogError("MakeSound given to object " + gameObject.name + " with no audio source. No sounds will be made.");
    }

    public void Play(AudioClip audio)
    {
        if(source == null)
        {
            Debug.LogError("MakeSound given to object " + gameObject.name + " with no audio source. No sounds will be made.");
            return;
        }
        source.PlayOneShot(audio);
    }

    public void Play(AudioClip audio, bool loop)
    {
        if (source == null)
        {
            Debug.LogError("MakeSound given to object " + gameObject.name + " with no audio source. No sounds will be made.");
            return;
        }
        source.loop = loop;
        source.clip = audio;
        source.Play();
    }
}
