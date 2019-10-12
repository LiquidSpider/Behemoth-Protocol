using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OldWeaponController : MonoBehaviour {

	[SerializeField] private GameObject missile;
	[SerializeField] private GameObject missileSpawnLocation;

	//[SerializeField] private GameObject bomb;

	[SerializeField] private GameObject flare;

	private Vector3 currentPosition;
	private Vector3 previousPosition;
	public Vector3 playerSpeed;

	public GameObject soundSrc;                 // The object that makes the sound
	public AudioClip sFire;                     // Sound played when gun fires

	//private float timeBetweenBombs = 1.5f;
	//private float timeOfLastBomb;

	private float timeBetweenMissiles = 10f;
	private float timeOfLastMissile;
	private float timeOfLastMissile2;

	private float timeBetweenFlares = 5f;
	private float timeOfLastFlare;
	private float timeOfLastFlare2;
	private float timeOfLastFlare3;
	private float timeOfLastFlare4;
	private float timeOfLastFlare5;


	private bool missileLock = false;
	private float timeOfLock;
	private float timeForUnlock = 2f;
	private GameObject missileTarget;



	private bool swordInUse = false;
	private float timeSwordAttackStart;


	void Start() {
		currentPosition = transform.GetChild(1).position;
		//timeOfLastBomb = -timeBetweenBombs;
		timeOfLastMissile = -timeBetweenMissiles;
		timeOfLastMissile2 = -timeBetweenMissiles;

		timeOfLastFlare = -timeBetweenFlares;
		timeOfLastFlare2 = -timeBetweenFlares;
		timeOfLastFlare3 = -timeBetweenFlares;
		timeOfLastFlare4 = -timeBetweenFlares;
		timeOfLastFlare5 = -timeBetweenFlares;
	}

	void Update() {
		previousPosition = currentPosition;
		currentPosition = transform.GetChild(1).position;
		playerSpeed = currentPosition - previousPosition;

		//playerSpeed = transform.GetChild(0).GetComponent<Rigidbody>().velocity * Time.deltaTime;


		if (swordInUse && timeSwordAttackStart + 1 < Time.time) {
			gameObject.transform.GetChild(5).GetComponent<Animator>().SetBool("attack", false);
			swordInUse = false;
		}

		if (!gameObject.GetComponent<PlayerController>().isCruising) {
			if (Input.GetButtonDown("Melee")) {
				swordInUse = true;

				gameObject.transform.GetChild(5).GetComponent<Animator>().SetBool("attack", true);
				timeSwordAttackStart = Time.time;
			}
		}


		//Things should not recharge if the battery is too low
		if (gameObject.GetComponent<PlayerHealth>().battery > 500) {
			// Take energy away from the battery if the missiles have been used (and recharge the missiles)
			if (Time.time < timeOfLastMissile + timeBetweenMissiles) {
				gameObject.GetComponent<PlayerHealth>().UseBattery(( 500 / timeBetweenMissiles ) * Time.deltaTime);
				GameObject.FindGameObjectWithTag("LeftSelect").transform.GetChild(0).GetComponent<Image>().fillAmount = 1 - ( timeOfLastMissile + timeBetweenMissiles - Time.time ) / timeBetweenMissiles;
			}
			if (Time.time < timeOfLastMissile2 + timeBetweenMissiles) {
				gameObject.GetComponent<PlayerHealth>().UseBattery(( 500 / timeBetweenMissiles ) * Time.deltaTime);
				GameObject.FindGameObjectWithTag("LeftSelect").transform.GetChild(1).GetComponent<Image>().fillAmount = 1 - ( timeOfLastMissile2 + timeBetweenMissiles - Time.time ) / timeBetweenMissiles;
			}

			// Same as above (Missiles) But for Flares
			if (Time.time < timeOfLastFlare + timeBetweenFlares) {
				gameObject.GetComponent<PlayerHealth>().UseBattery(( 200 / timeBetweenFlares ) * Time.deltaTime);
				GameObject.FindGameObjectWithTag("LeftSelect").transform.GetChild(2).GetComponent<Image>().fillAmount = 1 - ( timeOfLastFlare + timeBetweenFlares - Time.time ) / timeBetweenFlares;
			}
			if (Time.time < timeOfLastFlare2 + timeBetweenFlares) {
				gameObject.GetComponent<PlayerHealth>().UseBattery(( 200 / timeBetweenFlares ) * Time.deltaTime);
				GameObject.FindGameObjectWithTag("LeftSelect").transform.GetChild(3).GetComponent<Image>().fillAmount = 1 - ( timeOfLastFlare2 + timeBetweenFlares - Time.time ) / timeBetweenFlares;
			}
			if (Time.time < timeOfLastFlare3 + timeBetweenFlares) {
				gameObject.GetComponent<PlayerHealth>().UseBattery(( 200 / timeBetweenFlares ) * Time.deltaTime);
				GameObject.FindGameObjectWithTag("LeftSelect").transform.GetChild(4).GetComponent<Image>().fillAmount = 1 - ( timeOfLastFlare3 + timeBetweenFlares - Time.time ) / timeBetweenFlares;
			}
			if (Time.time < timeOfLastFlare4 + timeBetweenFlares) {
				gameObject.GetComponent<PlayerHealth>().UseBattery(( 200 / timeBetweenFlares ) * Time.deltaTime);
				GameObject.FindGameObjectWithTag("LeftSelect").transform.GetChild(5).GetComponent<Image>().fillAmount = 1 - ( timeOfLastFlare4 + timeBetweenFlares - Time.time ) / timeBetweenFlares;
			}
			if (Time.time < timeOfLastFlare5 + timeBetweenFlares) {
				gameObject.GetComponent<PlayerHealth>().UseBattery(( 200 / timeBetweenFlares ) * Time.deltaTime);
				GameObject.FindGameObjectWithTag("LeftSelect").transform.GetChild(6).GetComponent<Image>().fillAmount = 1 - ( timeOfLastFlare5 + timeBetweenFlares - Time.time ) / timeBetweenFlares;
			}
		}

		if (!transform.GetComponentInChildren<PlayerController>().isCruising && !swordInUse) {
			//if (GameObject.FindGameObjectWithTag("LeftSelect").GetComponent<WeaponSelect>().weaponNumber == 2) {
			if (GameObject.FindGameObjectWithTag("UI").transform.GetChild(2).GetComponent<WeaponSelect>().weaponNumber == 2) {
				if (Input.GetButtonDown("Attack")) {
					if (Time.time > timeOfLastMissile + timeBetweenMissiles) {
						missileSpawnLocation = GameObject.FindGameObjectWithTag("CurrentWeapon").transform.GetChild(0).GetChild(0).gameObject;

						timeOfLastMissile = Time.time;
						LaunchMissile();
					} else if (Time.time > timeOfLastMissile2 + timeBetweenMissiles) {
						missileSpawnLocation = GameObject.FindGameObjectWithTag("CurrentWeapon").transform.GetChild(0).GetChild(0).gameObject;

						timeOfLastMissile2 = Time.time;
						LaunchMissile();
					}
				}
			} else if (GameObject.FindGameObjectWithTag("UI").transform.GetChild(2).GetComponent<WeaponSelect>().previousWeaponNumber == 2) {
			//} else if (GameObject.FindGameObjectWithTag("LeftSelect").GetComponent<WeaponSelect>().previousWeaponNumber == 2) {
				missileSpawnLocation = null;
			}

			// Launch Flares when player presses Q, each one keeps track of whether its been fired
			if (Input.GetButtonDown("Flare")) {
				if (gameObject.GetComponent<PlayerHealth>().battery > 500) {
					if (Time.time > timeOfLastFlare + timeBetweenFlares) {

						timeOfLastFlare = Time.time;
						LaunchFlare();
					} else if (Time.time > timeOfLastFlare2 + timeBetweenFlares) {

						timeOfLastFlare2 = Time.time;
						LaunchFlare();
					} else if (Time.time > timeOfLastFlare3 + timeBetweenFlares) {

						timeOfLastFlare3 = Time.time;
						LaunchFlare();
					} else if (Time.time > timeOfLastFlare4 + timeBetweenFlares) {

						timeOfLastFlare4 = Time.time;
						LaunchFlare();
					} else if (Time.time > timeOfLastFlare5 + timeBetweenFlares) {

						timeOfLastFlare5 = Time.time;
						LaunchFlare();
					}
				}
			}

		}
		// Commented out because Bombs may not be a thing anymore
		/*else {
			if (Input.GetKeyDown(KeyCode.B) && Time.time > timeOfLastBomb + timeBetweenBombs) {
				timeOfLastBomb = Time.time;
				LaunchBomb();
			}
		} */

		RaycastHit hit;
		if (Physics.Raycast(Camera.main.transform.position, Camera.main.transform.forward, out hit)) {
			if (hit.collider.gameObject.tag != "Environment") {
				missileLock = true;

				missileTarget = hit.collider.gameObject;

				timeOfLock = Time.time;
			} else {
				if (timeOfLock + timeForUnlock > Time.time) {
					missileLock = false;
					missileTarget = null;
				}
			}
		}
	}

	private void LaunchMissile() {
		GameObject newMissile = Instantiate(missile);
		MakeSound(sFire, false);
		newMissile.transform.position = missileSpawnLocation.transform.position;

		if (missileLock) newMissile.GetComponent<MissileBehaviour>().Initialise(gameObject, missileTarget.gameObject, missileLock);
		else newMissile.GetComponent<MissileBehaviour>().Initialise(gameObject, Camera.main.transform.position);

		newMissile.GetComponent<MissileBehaviour>().playerSpeed = playerSpeed;
		newMissile.transform.GetChild(0).GetComponent<TrailRenderer>().material.color = Color.cyan;
	}

	//private void LaunchBomb() {
	//	GameObject newBomb = Instantiate(bomb);

	//	Vector3 spawnLocation = transform.GetChild(1).position;
	//	spawnLocation.y -= 1.5f;
	//	newBomb.transform.position = spawnLocation;

	//	newBomb.GetComponent<BombBehaviour>().Initialise(Vector3.zero);
	//}

	private void LaunchFlare() {
		for (int i = 1; i <= 2; i++) {
			GameObject newFlare = Instantiate(flare);

			Vector3 spawnLocation = transform.GetChild(0).position;
			spawnLocation.y -= 1.5f;
			spawnLocation += transform.right * Mathf.Pow(-1, i);
			newFlare.transform.position = spawnLocation;

			newFlare.transform.GetChild(0).GetComponent<FlareBehaviour>().Initialise(transform.GetChild(0).right * Mathf.Pow(-1, i));
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
