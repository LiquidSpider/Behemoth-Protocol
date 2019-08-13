using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UISounds : MonoBehaviour
{
    public List<AudioClip> hMarkerSounds = new List<AudioClip>();
    public AudioClip replenish;
    [Tooltip("Replenish lowest pitch")]
    public float rminPitch = 1f;
    [Tooltip("Replenish highest pitch")]
    public float rmaxPitch = 1.25f;
    [Tooltip("The fastest the replenish sound can play (to reduce spam)")]
    public float rdelay = 0.15f;
    public AudioClip shieldActivate;
    public float hdelay = 0.2f;
    public AudioClip shieldHit;


    // Elements:
    // 0 = Hitmarker
    // 1 = Replenish
    // 2 = Shield
    public List<AudioSource> sources = new List<AudioSource>();


    private float rtime = 0;
    private float htime = 0;

    private void Update()
    {
        rtime += Time.deltaTime;
        htime += Time.deltaTime;
    }

    public void HitMarker()
    {
        Debug.Log("Hitmarker sound was attempted");
        sources[0].PlayOneShot(hMarkerSounds[Random.Range(0, 4)]);
    }


    public void Replenish()
    {
        if (rtime > 0.1f)
        {
            sources[1].pitch = Random.Range(rminPitch, rmaxPitch);
            sources[1].PlayOneShot(replenish);
            rtime = 0;
        }
    }

    public void ShieldActivate()
    {
        sources[2].PlayOneShot(shieldActivate);
    }

    public void ShieldHit()
    {
        if (htime > hdelay)
        {
            sources[2].PlayOneShot(shieldHit);
            htime = 0;
        }
    }
}
