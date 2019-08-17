using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicZone : MonoBehaviour
{
    public MusicManager.Music song = MusicManager.Music.Combat;
    public float volume = 1f;
    private MusicManager music;
    // Start is called before the first frame update
    void Start()
    {
        music = FindObjectOfType<MusicManager>();
    }

    void OnTriggerEnter(Collider collision)
    {
        if(collision.gameObject.tag == "Player") //collision.gameObject.GetComponent<PlayerHealth>()
        {
            Debug.Log("Player has entered zone: " + song.ToString());
            if (music.CurrentSong() != song)
            {
                music.SetSong(song, volume);
            }
        }
    }
}
