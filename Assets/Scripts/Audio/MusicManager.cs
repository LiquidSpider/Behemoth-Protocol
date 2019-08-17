using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicManager : MonoBehaviour
{
    public List<AudioClip> bgm = new List<AudioClip>();

    /// <summary>
    /// Song list:
    /// 0 = Intro music (Simple)
    /// 1 = Combat music (damn_robot_2)
    /// </summary>

    public enum Music { Intro, Combat };
    public float musicVolume = 1; // Better off just modifying the mixer for this.
    private Music currentSong = Music.Intro;
    private AudioSource source;
    private AudioClip targetSong;

    [Tooltip("Speed of fade transition")]
    public float fadeSpeed = 1;
    private float timer;
    private bool transition = false;
    // Start is called before the first frame update
    void Start()
    {
        SetSong(currentSong, 0.8f);

        Debug.Log("MusicManager looking for audio source...");
        if (GetComponent<AudioSource>())
        {
            Debug.Log("MusicManager source OK");
            source = gameObject.GetComponent<AudioSource>();
        }
        else
        {
            Debug.Log("MusicManager source not found. Please ensure AudioSource exists on MusicManager.");
        }
        
    }

    // Update is called once per frame
    void Update()
    {
        Transition();
    }

    void Transition()
    {
        if (source.clip != targetSong)
        {
            source.volume -= Time.deltaTime * fadeSpeed;

            if (source.volume <= 0)
            {
                source.clip = targetSong;
                source.volume = musicVolume;
                source.Play();
            }
        }
    }

    public Music CurrentSong()
    {
        return currentSong;
    }

    /// <summary>
    /// Change the currently playing background music.
    /// </summary>
    /// <param name="music"></param>
    public void SetSong(Music music, float volume)
    {
        switch (music)
        {
            case Music.Intro:
                targetSong = bgm[0];
                break;
            case Music.Combat:
                targetSong = bgm[1];
                break;
        }
        musicVolume = volume;
    }
}
