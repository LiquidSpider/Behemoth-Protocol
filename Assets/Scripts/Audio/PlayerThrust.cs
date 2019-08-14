using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerThrust : MonoBehaviour
{

    private Rigidbody rb;
    private AudioSource audio;
    public AudioClip dodge;
    [Range(0, 1)]
    public float maxVolume = 0.8f;
    public float volmultoffset = 1;
    // Start is called before the first frame update
    void Start()
    {
        rb = FindObjectOfType<PlayerHealth>().gameObject.GetComponent<Rigidbody>();
        audio = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        float nVol;
        nVol = rb.velocity.magnitude * volmultoffset;
        if (nVol > maxVolume) nVol = maxVolume;
        audio.volume = nVol;
    }

    public void Dodge()
    {
        audio.PlayOneShot(dodge);
    }
}
