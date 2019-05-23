using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UISounds : MonoBehaviour
{
    public List<AudioClip> hMarkerSounds = new List<AudioClip>();
    public GameObject sndSrc;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void HitMarker()
    {
        GameObject snd = Instantiate(sndSrc, GameObject.FindGameObjectWithTag("Player").transform);
        snd.GetComponent<AudioSource>().spatialBlend = 0f;
        snd.GetComponent<AudioSource>().clip = hMarkerSounds[Random.Range(0, 4)];
        snd.GetComponent<AudioSource>().volume = 1;
        snd.transform.localPosition = Vector3.zero;
        snd.GetComponent<AudioSource>().Play();
        snd.GetComponent<AudioSource>().bypassListenerEffects = true;
        snd.GetComponent<TimedDestroy>().maxTime = snd.GetComponent<AudioSource>().clip.length;
    }
}
