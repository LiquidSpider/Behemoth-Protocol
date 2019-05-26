using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponController : MonoBehaviour {

	[SerializeField] private GameObject missile;
	[SerializeField] private GameObject missileSpawnLocation;

	[SerializeField] private GameObject bomb;

	[SerializeField] private GameObject flare;

	private Vector3 currentPosition;
	private Vector3 previousPosition;
	public Vector3 playerSpeed;

	public GameObject soundSrc;                 // The object that makes the sound
	public AudioClip sFire;                     // Sound played when gun fires

	private float timeBetweenBombs = 1.5f;
	private float timeOfLastBomb;

	private float timeBetweenMissiles = 2f;
	private float timeOfLastMissile;

	private float timeBetweenFlares = 0.5f;
	private float timeOfLastFlare;

	void Start() {
		currentPosition = transform.GetChild(1).position;
		timeOfLastBomb = -timeBetweenBombs;
		timeOfLastMissile = -timeBetweenMissiles;
		timeOfLastFlare = -timeBetweenFlares;
	}

	void Update() {
		previousPosition = currentPosition;
		currentPosition = transform.GetChild(1).position;
		playerSpeed = currentPosition - previousPosition;

		if (!transform.GetChild(1).GetComponent<PlayerController>().isCruising) {
			if (GameObject.FindGameObjectWithTag("LeftSelect").GetComponent<WeaponSelect>().weaponNumber == 2) {
				if (gameObject.GetComponent<PlayerHealth>().battery > 1000) {
					if (Input.GetButtonDown("Attack") && Time.time > timeOfLastMissile + timeBetweenMissiles) {
						missileSpawnLocation = GameObject.FindGameObjectWithTag("CurrentWeapon").transform.GetChild(0).GetChild(0).gameObject;

						timeOfLastMissile = Time.time;
						LaunchMissile();
					}
				}
			} else if (GameObject.FindGameObjectWithTag("LeftSelect").GetComponent<WeaponSelect>().previousWeaponNumber == 2) {
				missileSpawnLocation = null;
			}

			//if (Input.GetKeyDown(KeyCode.Q) && Time.time > timeOfLastFlare + timeBetweenFlares) {
			//	timeOfLastFlare = Time.time;
			//	LaunchFlare();
			//}

		} else {
			if (Input.GetKeyDown(KeyCode.B) && Time.time > timeOfLastBomb + timeBetweenBombs) {
				timeOfLastBomb = Time.time;
				LaunchBomb();
			}
		}
	}

	private void LaunchMissile() {
		GameObject newMissile = Instantiate(missile);
		MakeSound(sFire, false);
		newMissile.transform.position = missileSpawnLocation.transform.position;

		newMissile.GetComponent<MissileBehaviour>().Initialise(gameObject, Camera.main.transform.position);
		newMissile.GetComponent<MissileBehaviour>().playerSpeed = playerSpeed;
		newMissile.transform.GetChild(0).GetComponent<TrailRenderer>().material.color = Color.cyan;

		gameObject.GetComponent<PlayerHealth>().UseBattery(1500);
	}

	private void LaunchBomb() {
		GameObject newBomb = Instantiate(bomb);

		Vector3 spawnLocation = transform.GetChild(1).position;
		spawnLocation.y -= 1.5f;
		newBomb.transform.position = spawnLocation;

		newBomb.GetComponent<BombBehaviour>().Initialise(Vector3.zero);
	}

	private void LaunchFlare() {
		for (int i = 1; i <= 2; i++) {
			GameObject newFlare = Instantiate(flare);

			Vector3 spawnLocation = transform.GetChild(1).position;
			spawnLocation.y -= 1.5f;
			spawnLocation += transform.right * Mathf.Pow(-1, i);
			newFlare.transform.position = spawnLocation;

			//newFlare.GetComponent<FlareBehaviour>().Initialise(transform.right * Mathf.Pow(-1, i));
		}
	}
	void MakeSound(AudioClip sound, bool pitchRandom) {
		GameObject oSound = Instantiate(soundSrc, missileSpawnLocation.transform.position, Quaternion.identity);
		AudioSource source = oSound.GetComponent<AudioSource>();
		source.clip = sFire;
		source.volume = 0.75f;
		source.rolloffMode = AudioRolloffMode.Logarithmic;
		source.spatialBlend = 0;
		if (pitchRandom) source.pitch = Random.Range(0.75f, 1.25f);
		oSound.GetComponent<TimedDestroy>().maxTime = source.clip.length;
		oSound.transform.parent = GameObject.FindGameObjectWithTag("MissileParent").transform;
		source.Play();
	}
}
