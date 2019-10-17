using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class ExplosionBehaviour : MonoBehaviour {

	private float timeOfExplosion;
	private float explosionTime = 0.8f;
	public GameObject sndsrc;
	public AudioClip sExplosion;
	private float fadeStartTime = -1;
	private float fadeTime = 1f;
	public float sMinDist = 1f;     // Distance at which sound is loudest
	public float sMaxDist = 500f;   // Distance at which sound is inaudible
    public AudioMixerGroup mixer;

	void Start() {
		transform.parent = GameObject.FindGameObjectWithTag("ExplosionParent").transform;
		timeOfExplosion = Time.time;
		MakeSound(sExplosion, true);
	}

	void Update() {
		if (timeOfExplosion + explosionTime > Time.time) {
			float scaleFactor = (Time.time - timeOfExplosion) * Time.timeScale;
			transform.localScale += new Vector3(scaleFactor, scaleFactor, scaleFactor);
		} else  if (timeOfExplosion + fadeTime > Time.time) {
			if (fadeStartTime == -1) fadeStartTime = Time.time;

			Color old = GetComponent<MeshRenderer>().material.color;
			old.a = 1 - ( ( Time.time - fadeStartTime ) / (fadeTime - explosionTime) );

			GetComponent<MeshRenderer>().material.color = old;

			float scaleFactor = (Time.time - timeOfExplosion) * Time.timeScale;
			transform.localScale += new Vector3(scaleFactor, scaleFactor, scaleFactor);
		} else {
			Destroy(gameObject);
		}

		//if (Time.time > fadeStartTime + fadeTime) {
		//	if (Time.time > timeOfExplosion + explosionTime) {
		//		if (fadeStartTime == -1) {
		//			fadeStartTime = Time.time;
		//		}

		//		Color old = GetComponent<MeshRenderer>().material.color;
		//		old.a = 1 - (( Time.time - fadeStartTime) / fadeTime);

		//		GetComponent<MeshRenderer>().material.color = old;
		//	} else {
		//		float scaleFactor = (Time.time - timeOfExplosion) * Time.timeScale;
		//		transform.localScale += new Vector3(scaleFactor, scaleFactor, scaleFactor);
		//	}
		//} else {
		//	Destroy(gameObject);
		//}
	}

	void MakeSound(AudioClip sound, bool pitchRandom) {
		Vector3 pos = transform.position;
		GameObject oSound = Instantiate(sndsrc, pos, Quaternion.identity);
		AudioSource source = oSound.GetComponent<AudioSource>();
		source.clip = sExplosion;
        source.outputAudioMixerGroup = mixer;
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
