using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerThrust : MonoBehaviour
{

    private Rigidbody rb;
    private AudioSource audio;
    private AudioSource burnSource;
    public AudioClip dodge;
    public AudioClip disengage;
    public AudioClip afterburn;
    public AudioClip afterburning;
    [Range(0, 1)]
    public float maxVolume = 0.8f;
    public float volmultoffset = 1;
    private PlayerController player;
    private bool hasEngaged = false;
    // Start is called before the first frame update
    void Start()
    {
        player = FindObjectOfType<PlayerController>();
        rb = FindObjectOfType<PlayerHealth>().gameObject.GetComponent<Rigidbody>();
        audio = GetComponent<AudioSource>();
        burnSource = transform.GetChild(0).GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        float nVol;
        nVol = rb.velocity.magnitude * volmultoffset;
        if (nVol > maxVolume) nVol = maxVolume;
        audio.volume = nVol;
        Afterburn();
    }

    public void Dodge()
    {
        audio.PlayOneShot(dodge);
    }

    void Afterburn()
    {
        if (player.isCruising)
        {
            if (!hasEngaged)
            {
                burnSource.PlayOneShot(afterburn);
                hasEngaged = true;
                burnSource.clip = afterburning;
                burnSource.loop = true;
                burnSource.Play();
            }

            if (!burnSource.isPlaying) burnSource.Play();
        }
        else
        {
            if (hasEngaged)
            {
                hasEngaged = false;
                burnSource.clip = disengage;
                burnSource.loop = false;
                burnSource.Play();
            }
        }
    }
}
