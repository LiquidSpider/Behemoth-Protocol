using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MusicZone : MonoBehaviour
{
    public MusicManager.Music song = MusicManager.Music.Combat;
    public float volume = 1f;
    private MusicManager music;

    private GameObject giant;
    // Start is called before the first frame update
    void Start()
    {
        music = FindObjectOfType<MusicManager>();

        // Get the giant.
        giant = FindObjectOfType<giantBehaviour>().gameObject;
    }

    void OnTriggerEnter(Collider collision)
    {
        if(collision.gameObject.tag == "Player") //collision.gameObject.GetComponent<PlayerHealth>()
        {
            Debug.Log("Player has entered zone: " + song.ToString());
            if (music.CurrentSong() != song)
            {
                music.SetSong(song, volume);
                // Enabled the giant script.
                giant.GetComponent<giantBehaviour>().enabled = true;
            }

			// Navigator Prompt
			promptText.text = "You need to stop it from reaching the wall! Destroy it's legs to stop it moving.";
			promptText.gameObject.transform.parent.gameObject.SetActive(true);
		}
    }

	public Text promptText;
}
