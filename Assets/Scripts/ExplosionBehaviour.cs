using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExplosionBehaviour : MonoBehaviour {

	private float timeOfExplosion;
	private float explosionTime = 0.8f;
    public GameObject sndsrc;
    public AudioClip sExplosion;
	private float fadeStartTime = -1;
	private float fadeTime = 2f;
    public float sMinDist = 1f;     // Distance at which sound is loudest
    public float sMaxDist = 500f;   // Distance at which sound is inaudible

	void Start() {
		transform.parent = GameObject.FindGameObjectWithTag("ExplosionParent").transform;
		timeOfExplosion = Time.time;
        MakeSound(sExplosion, true);
	}
	
	void Update() {
		if (Time.time > fadeStartTime + fadeTime) {
			if (Time.time > timeOfExplosion + explosionTime) {
				if (fadeStartTime == -1) {
					fadeStartTime = Time.time;
				}

				Color old = GetComponent<MeshRenderer>().material.color;
				old.a = old.a - ( Time.deltaTime / fadeTime );

				GetComponent<MeshRenderer>().material.color = old;
			} else {
				float scaleFactor = Time.time - timeOfExplosion;
				transform.localScale += new Vector3(scaleFactor, scaleFactor, scaleFactor);
			}
		} else {
			Destroy(gameObject);
		}
	}
    void MakeSound(AudioClip sound, bool pitchRandom)
    {
        Vector3 pos = transform.position;
        GameObject oSound = Instantiate(sndsrc, pos, Quaternion.identity);
        AudioSource source = oSound.GetComponent<AudioSource>();
        source.clip = sExplosion;
        source.volume = 1;
        source.minDistance = sMinDist;
        source.maxDistance = sMaxDist;
        if (pitchRandom) source.pitch = Random.Range(0.75f, 1.25f);
        oSound.GetComponent<TimedDestroy>().maxTime = source.clip.length;
        oSound.transform.parent = GameObject.FindGameObjectWithTag("MissileParent").transform;
        source.volume = 0.7f;
        source.rolloffMode = AudioRolloffMode.Logarithmic;
        source.Play();
    }
}
